#!/bin/bash
# Запуск SQL Server в фоновом режиме
/opt/mssql/bin/sqlservr &
# Ожидание запуска сервера
sleep 30
# Создание базы данных
/opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $MSSQL_SA_PASSWORD -Q "CREATE DATABASE toplivoEF"
# Бесконечное ожидание чтобы контейнер продолжал работу
wait