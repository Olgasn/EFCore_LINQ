#!/bin/bash
set -e

# Ждём, пока SQL Server будет готов принимать подключения
echo "Ожидание готовности SQL Server..."
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  echo "SQL Server ещё не готов, ждём..."
  sleep 5
done

echo "SQL Server готов. Создаём базу данных..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -i /tmp/setup-database.sql
echo "Инициализация завершена."