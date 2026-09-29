# 🛒 E-commerce Microservices Project

A distributed, modular e-commerce platform built with .NET 10 and modern microservice practices. It includes user profiles, product catalog, cart and order processing, payments with Stripe, and real-time notifications — authenticated with Keycloak and containerized with Docker.

## 📚 Table of Contents

- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Authentication](#-authentication-keycloak)
- [Services Breakdown](#-services-breakdown)
- [Getting Started](#-getting-started)
- [Known Limitations](#-known-limitations)

## 🧱 Architecture

- **Microservices** – Each feature is its own service, each with its own database.
- **RESTful APIs** – Public endpoints for external access.
- **gRPC** – Internal, synchronous communication between services.
- **AMQP** – RabbitMQ (via MassTransit) for asynchronous, event-driven communication.
- **OIDC / Keycloak** – Identity, authentication and roles are delegated to Keycloak; services validate JWTs independently (zero-trust between services).
- **API Gateway** – YARP routes external requests to the right service.
- **Dockerized** – Every service and its infrastructure (Postgres, Redis, RabbitMQ, Keycloak) run in containers via Docker Compose.

```
Client ⇄ Keycloak (login, tokens)
Client ⇄ API Gateway ⇄ Microservices
                       ↳ User Service        (profiles, addresses, seller accounts)
                       ↳ Product Service     (catalog)
                       ↳ Cart Service        (cart, checkout)
                       ↳ Order Service       (orders)
                       ↳ Payment Service     (Stripe charges via gRPC)
                       ↳ Notification Service (SignalR + notifications)
```

Each of User, Product, Cart, Order and Notification Service validates the access token independently against Keycloak's public keys — there is no shared session state and no service trusts another service's word about who the caller is.

---

## 🧰 Tech Stack

- **.NET 10** (ASP.NET Core)
- **Keycloak** – Identity provider (OIDC, Authorization Code + PKCE)
- **PostgreSQL**, **Redis**
- **REST API**, **gRPC**, **SignalR**
- **RabbitMQ**, **MassTransit** – Async messaging
- **Stripe** – Payments
- **YARP** – API Gateway
- **Docker**, **Docker Compose**
- **Libs:** EF Core, Marten, Mapster, AutoMapper, MediatR, FluentValidation, Carter, BCrypt.Net, Scrutor

---

## 🔐 Authentication (Keycloak)

Identity is fully delegated to Keycloak. **No service issues, stores, or validates passwords** — UserService only stores profile, address, and seller-account data keyed by the Keycloak user ID (`sub`).

**Flow used:** Authorization Code + PKCE. The client (frontend/Postman) authenticates directly against Keycloak and never sends a password to any of these APIs.

```
Client → Keycloak /protocol/openid-connect/auth  (login page, PKCE challenge)
Client ← access_token + refresh_token
Client → any service, Authorization: Bearer <access_token>
Service → validates the token against Keycloak's JWKS (Authority/Audience), independently, per request
```

- **Realm:** `ecommerce` — imported automatically from [`keycloak/realm-export.json`](./keycloak/realm-export.json) on first startup (`--import-realm`).
- **Client:** `ecommerce-frontend` — public client, PKCE-only (no client secret, no password grant).
- **Roles:** `customer`, `seller`, `admin` (realm roles, read from `realm_access.roles` in the token and surfaced as ASP.NET Core role claims).
- **Seller access:** the `seller` role gates seller-only endpoints (creating/updating products, managing a seller account). Product ownership is enforced by comparing the resource's owner ID against the caller's `sub` — the same ID is used as both the user ID and the seller ID, so no separate `seller_id` claim is needed.
- **Assigning the `seller` role is a manual, admin-only step** for this demo — creating a `SellerAccount` via the API does **not** automatically grant the role. To test seller endpoints: create a seller account, then in the Keycloak admin console (`http://localhost:8081/admin`) go to **Users → (user) → Role mapping → Assign role → seller**, then obtain a fresh token (roles are baked into the token at issue time).
- **Account management** (change password, update email/name, verify email) is handled by Keycloak directly — see its [Account REST API](https://www.keycloak.org/docs/latest/securing_apps/#account-rest-api) or the hosted account console at `/realms/ecommerce/account`. These APIs no longer expose register/login/change-password endpoints.

**Seeded test users** (from the realm export, for local testing):

| Username    | Password    | Roles                | Notes                            |
| ----------- | ----------- | -------------------- | -------------------------------- |
| `customer1` | `Passw0rd!` | `customer`           | No seller access                 |
| `seller1`   | `Passw0rd!` | `customer`, `seller` | Has the seller role pre-assigned |

---

## 🧩 Services Breakdown

### 🧑 [User Service](./src/Services/UserService)

**Patterns:** Clean Architecture, CQRS
Stores only profile/address/seller data. Identity, credentials and roles live entirely in Keycloak.

**Profile & Address Endpoints** _(Auth Required)_

- `POST /api/users/me/addresses`
- `GET /api/users/me/addresses`
- `GET /api/users/me/addresses/{id}`
- `PUT /api/users/me/addresses/{id}`

**Seller Account Endpoints**

- `POST /api/users/me/seller` _(Auth Required)_ — create a seller account for the current user
- `GET /api/users/me/seller` _(Auth Required, Role: `seller`)_
- `PUT /api/users/me/seller` _(Auth Required, Role: `seller`)_
- `GET /api/sellers/{id}` _(Anonymous)_ — public seller profile

---

### 🛍 [Product Service](./src/Services/ProductService)

**Patterns:** Vertical Slice Architecture (VSA), CQRS

**API Endpoints**

- `POST /api/products` _(Auth Required, Role: `seller`)_
- `PUT /api/products/{id}` _(Auth Required, Role: `seller`, must own the product)_
- `GET /api/products/{id}`
- `GET /api/products?search=&category=&minPrice=&maxPrice=&pageSize=&pageNumber=`

**gRPC:** `GetProduct()`

---

### 🛒 [Cart Service](./src/Services/CartService)

**Patterns:** Vertical Slice Architecture (VSA), Repository Pattern, CQRS

**API Endpoints** _(Auth Required)_

- `GET /api/cart`
- `POST /api/cart/items`
- `PUT /api/cart/items/{id}`
- `DELETE /api/cart/items/{id}`
- `DELETE /api/cart`
- `POST /api/cart/checkout`

**RabbitMQ Events:** `CartCheckoutEvent` (Publisher)

---

### 💳 [Payment Service](./src/Services/PaymentService)

**gRPC:** `Charge()`

**API Endpoints**

- `GET /api/payment/key` — get Stripe public key

---

### 📦 [Order Service](./src/Services/OrderService)

**Patterns:** Vertical Slice Architecture (VSA), CQRS

**API Endpoints** _(Auth Required)_

- `GET /api/orders`
- `GET /api/orders/{id}`
- `PUT /api/orders/{id}/cancel`

**RabbitMQ Events:**

- `CreateOrderConsumer` (Consumer, listens for `CartCheckoutEvent`)
- `NotificationCreatedEvent` (Publisher)

---

### 🔔 [Notification Service](./src/Services/NotificationService)

**Patterns:** VSA, CQRS

**API Endpoints** _(Auth Required)_

- `GET /api/notifications`
- `PUT /api/notifications`
- `PUT /api/notifications/{id}`

**Hubs:**

- `/api/notifications/hub` _(SignalR)_

**RabbitMQ Events:** `NotificationCreatedEvent` (Consumer)

---

### 🌐 [API Gateway](./src/ApiGateway)

**Tool:** YARP Reverse Proxy
**Routes:**

- `/user-service/{**catch-all}`
- `/product-service/{**catch-all}`
- `/cart-service/{**catch-all}`
- `/payment-service/{**catch-all}`
- `/order-service/{**catch-all}`
- `/notification-service/{**catch-all}`

The gateway forwards the `Authorization` header unchanged — it does not itself validate tokens; each downstream service validates independently.

---

### 📦 [Building Blocks (Shared)](./src/BuildingBlocks)

Shared cross-cutting code used by every service:

- `AuthenticationExtension` — configures JWT Bearer validation against Keycloak (Authority/Audience/JWKS discovery)
- `KeycloakRolesClaimsTransformation` — flattens Keycloak's `realm_access.roles` into standard ASP.NET Core role claims
- `IUserContext` / `CurrentUser` — exposes the authenticated user's ID (`sub`) and roles to application code
- Global exception handling middleware, common constants, events, and gRPC proto contracts

---

## 🚀 Getting Started

### Prerequisites

- Docker
- Docker Compose
- .NET SDK (for local development and building)

### 📦 Docker Compose Setup

```bash
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
```

On first run, Keycloak imports the `ecommerce` realm automatically from `keycloak/realm-export.json`, including the `ecommerce-frontend` client and the two seeded test users above.

> If you change `keycloak/realm-export.json` after the first run, Keycloak will **not** re-import it — `--import-realm` skips realms that already exist. Remove the `keycloak` container (and its data, if you added a volume) and start it again:
>
> ```bash
> docker compose rm -sf keycloak
> docker compose up -d keycloak
> ```

### Getting a token for testing

Since the client is PKCE-only (no password grant), use a tool that supports the Authorization Code + PKCE flow, e.g. Postman:

- Grant type: `Authorization Code (With PKCE)`
- Auth URL: `http://localhost:8081/realms/ecommerce/protocol/openid-connect/auth`
- Access Token URL: `http://localhost:8081/realms/ecommerce/protocol/openid-connect/token`
- Client ID: `ecommerce-frontend`
- Code Challenge Method: `SHA-256`
- Callback URL: `http://localhost:5173/callback` (or any URL configured as a redirect URI in the realm export)

Log in as `customer1` or `seller1` and use the resulting `access_token` as a `Bearer` token against any service.

### 🐳 Services & Ports

> _Note: Local = host machine, Docker = mapped container ports, Inside = internal container ports._

| Service                 | Local Ports | Docker Ports | Docker-Inside Ports |
| ----------------------- | ----------- | ------------ | ------------------- |
| Keycloak                | 8081        | 8081         | 8080                |
| API Gateway             | 6060, 6061  | 6060, 6061   | 8080, 8081          |
| User Service            | 6000, 6001  | 6000, 6001   | 8080, 8081          |
| Product Service         | 6010, 6011  | 6010, 6011   | 8080, 8081          |
| Cart Service            | 6020, 6021  | 6020, 6021   | 8080, 8081          |
| Payment Service         | 6030, 6031  | 6030, 6031   | 8080, 8081          |
| Order Service           | 6040, 6041  | 6040, 6041   | 8080, 8081          |
| Notification Service    | 6050, 6051  | 6050, 6051   | 8080, 8081          |
| User Service DB         | 5432        | 5432         | 5432                |
| Product Service DB      | 5433        | 5433         | 5432                |
| Cart Service DB         | 5434        | 5434         | 5432                |
| Order Service DB        | 5435        | 5435         | 5432                |
| Notification Service DB | 5436        | 5436         | 5432                |
| Redis                   | 6379        | 6379         | 6379                |
| RabbitMQ                | 5672, 15672 | 5672, 15672  | 5672, 15672         |

> **Note**: All services run in development mode. Ensure HTTPS dev certs and user-secrets are available for .NET services (mounted from `%APPDATA%` in the override file).

### 🔐 Environment Variables

Each service expects its own set of environment variables, defined in `docker-compose.override.yml`:

- `ConnectionStrings__*` — Postgres/Redis connection strings
- `Keycloak__Authority`, `Keycloak__Audience`, `Keycloak__DevMode` — token validation settings (User, Product, Cart, Order, Notification Service)
- `RabbitMQ__Host`, `RabbitMQ__Username`, `RabbitMQ__Password` — message broker credentials
- `Stripe__PublishableKey`, `Stripe__SecretKey` — Payment Service

The committed values are local-dev-only placeholders (`mysecretpassword`, `guest`, `your_stripe_secret_key`, etc.). Replace them with real secrets via `.env`, user-secrets, or a secrets manager for anything beyond local development — never commit real credentials or private keys.

---

## ⚠️ Known Limitations

This project is a portfolio/demo, and a few things are intentionally out of scope:

- **Seller role assignment is manual** — see [Authentication](#-authentication-keycloak) above.
- **Local Keycloak realm is for development only** — the seeded users, client secret-less public client, and `sslRequired: external` setting are not production hardening.
