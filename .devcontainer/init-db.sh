#!/bin/bash
set -e

# Ждём, пока SQL Server будет готов принимать подключения
echo "Waiting for SQL Server to be ready..."
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  echo "SQL Server is not ready yet, waiting..."
  sleep 5
done

echo "SQL Server is ready. Creating the database..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -i /tmp/setup-database.sql
echo "Инициализация завершена."