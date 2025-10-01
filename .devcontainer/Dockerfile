# »спользуем официальный образ Ubuntu в качестве базового
FROM mcr.microsoft.com/devcontainers/base:ubuntu

# ”становка зависимостей и добавление репозитори€ Microsoft
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get install -y curl apt-transport-https gnupg2

# ƒобавл€ем репозиторий SQL Server и устанавливаем его
RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl -fsSL https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2022.list | tee /etc/apt/sources.list.d/mssql-server.list \
    && apt-get update \
    && apt-get install -y mssql-server

# ”станавливаем инструменты командной строки SQL Server (sqlcmd и bcp)
RUN curl -fsSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl -fsSL https://packages.microsoft.com/config/ubuntu/20.04/prod.list | tee /etc/apt/sources.list.d/msprod.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools18

# ƒобавл€ем mssql-tools в PATH дл€ удобства
RUN echo 'export PATH="$PATH:/opt/mssql-tools18/bin"' >> ~/.bashrc

# ќткрываем порт, который прослушивает SQL Server
EXPOSE 1433