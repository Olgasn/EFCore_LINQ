#!/bin/bash
set -e
echo "ќжидание запуска SQL Server..."
for i in {1..30}; do
    /opt/mssql-tools/bin/sqlcmd -S mssql -U sa -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" && exit 0
    sleep 2
done
echo "SQL Server не запущен!"
exit 1