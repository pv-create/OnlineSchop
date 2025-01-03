-- Создание баз данных
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ClientDb')
BEGIN
    CREATE DATABASE ClientDb;
END
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'OrderDb')
BEGIN
    CREATE DATABASE OrderDb;
END
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'SagaDb')
BEGIN
    CREATE DATABASE SagaDb;
END
GO