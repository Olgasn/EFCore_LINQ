#!/bin/bash
set -e

# ���, ���� SQL Server ����� ����� ��������� �����������
echo "�������� ���������� SQL Server..."
until /opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; do
  echo "SQL Server ��� �� �����, ���..."
  sleep 5
done

echo "SQL Server �����. ������ ���� ������..."
/opt/mssql-tools18/bin/sqlcmd -S localhost -U SA -P "$MSSQL_SA_PASSWORD" -i /tmp/setup-database.sql
echo "������������� ���������."