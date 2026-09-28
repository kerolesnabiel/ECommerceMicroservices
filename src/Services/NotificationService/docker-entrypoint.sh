#!/bin/sh

echo "Waiting for PostgreSQL to be ready..."
./wait-for notification-service-db:5432

echo "Waiting for Redis to be ready..."
./wait-for distributed-cache:6379

echo "Starting NotificationService..."
exec dotnet NotificationService.dll
