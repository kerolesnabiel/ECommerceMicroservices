#!/bin/sh

echo "Waiting for PostgreSQL to be ready..."
./wait-for cart-service-db:5432

echo "Waiting for Redis to be ready..."
./wait-for distributed-cache:6379

echo "Starting CartService..."
exec dotnet CartService.dll
