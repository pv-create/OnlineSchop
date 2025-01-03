USE ClientDb;
GO

-- Вставка тестовых клиентов
IF NOT EXISTS (SELECT TOP 1 1 FROM [dbo].[Clients])
BEGIN
    DECLARE @ClientId1 UNIQUEIDENTIFIER = NEWID();
    DECLARE @ClientId2 UNIQUEIDENTIFIER = NEWID();

    INSERT INTO [dbo].[Clients] ([Id], [Name], [Email], [IsActive], [CreditLimit])
    VALUES 
        (@ClientId1, 'John Doe', 'john@example.com', 1, 1000.00),
        (@ClientId2, 'Jane Smith', 'jane@example.com', 1, 2000.00);

    INSERT INTO [dbo].[ClientAddresses] ([ClientId], [Street], [City], [State], [Country], [PostalCode], [IsDefault])
    VALUES
        (@ClientId1, '123 Main St', 'New York', 'NY', 'USA', '10001', 1),
        (@ClientId2, '456 Park Ave', 'Los Angeles', 'CA', 'USA', '90001', 1);
END
GO