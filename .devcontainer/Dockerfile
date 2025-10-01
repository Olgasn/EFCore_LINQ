FROM mcr.microsoft.com/devcontainers/base:ubuntu-22.04

# Установка зависимостей
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get install -y curl apt-transport-https gnupg2

# Добавляем репозиторий SQL Server
RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/mssql-server-2022.list | tee /etc/apt/sources.list.d/mssql-server.list

# Устанавливаем SQL Server
RUN apt-get update && apt-get install -y mssql-server

# Устанавливаем инструменты командной строки
RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl -fsSL https://packages.microsoft.com/config/ubuntu/22.04/prod.list | tee /etc/apt/sources.list.d/msprod.list \
    && apt-get update && ACCEPT_EULA=Y apt-get install -y mssql-tools18

# Добавляем в PATH
ENV PATH="/opt/mssql-tools18/bin:$PATH"

# Копируем файлы
COPY setup-database.sql /tmp/
COPY init-db.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/init-db.sh

EXPOSE 1433

# Запускаем SQL Server как основной процесс
CMD ["/opt/mssql/bin/sqlservr"]