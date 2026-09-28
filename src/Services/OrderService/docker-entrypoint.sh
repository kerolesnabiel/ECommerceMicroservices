#!/bin/sh

echo "Waiting for PostgreSQL to be ready..."
./wait-for order-service-db:5432

echo "Starting OrderService..."
exec dotnet OrderService.dll
