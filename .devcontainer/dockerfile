FROM mcr.microsoft.com/vscode/devcontainers/base:ubuntu

RUN apt-get update && apt-get install -y curl apt-transport-https gnupg2
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/ubuntu/20.04/mssql-server-2022.list | tee /etc/apt/sources.list.d/mssql-server.list
RUN apt-get update && apt-get install -y mssql-server mssql-tools18

# Копирование и настройка скрипта запуска
COPY startup.sh /usr/local/bin/
RUN chmod +x /usr/local/bin/startup.sh

EXPOSE 1433
CMD ["/usr/local/bin/startup.sh"]
