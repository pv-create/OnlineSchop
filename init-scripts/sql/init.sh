#!/bin/bash
set -e

# Ожидание запуска SQL Server
sleep 30s

# Установка необходимых пакетов
apt-get update
apt-get install -y curl gnupg2

# Добавление репозитория Microsoft
curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
curl https://packages.microsoft.com/config/ubuntu/20.04/prod.list > /etc/apt/sources.list.d/mssql-release.list

# Установка SQL Server tools
apt-get update
ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev

# Добавление в PATH
echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc
source ~/.bashrc

# Запуск SQL Server в фоновом режиме
/opt/mssql/bin/sqlservr &

# Ожидание доступности SQL Server
sleep 30s

# Выполнение скриптов инициализации
for i in {1..50};
do
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -d master -i /docker-entrypoint-initdb.d/01-create-databases.sql && break
    sleep 1
done

# Выполнение остальных скриптов
for f in /docker-entrypoint-initdb.d/*-*.sql
do
    echo "Processing $f file..."
    /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SA_PASSWORD -i "$f"
done

# Бесконечный цикл для поддержания контейнера
tail -f /dev/null