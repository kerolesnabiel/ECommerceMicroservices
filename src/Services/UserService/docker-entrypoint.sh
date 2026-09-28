#!/bin/sh

echo "Waiting for PostgreSQL to be ready..."
./wait-for user-service-db:5432

echo "Starting UserService..."
exec dotnet UserService.dll
