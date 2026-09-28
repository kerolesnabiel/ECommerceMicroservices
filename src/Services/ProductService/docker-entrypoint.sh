#!/bin/sh

echo "Waiting for PostgreSQL to be ready..."
./wait-for product-service-db:5432

echo "Starting ProductService..."
exec dotnet ProductService.dll
