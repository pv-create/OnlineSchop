USE ClientDb;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clients]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Clients](
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [Name] NVARCHAR(100) NOT NULL,
        [Email] NVARCHAR(100) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT 1,
        [CreditLimit] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [UpdatedAt] DATETIME2 NULL
    );

    CREATE INDEX [IX_Clients_Email] ON [dbo].[Clients]([Email]);
END
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClientAddresses]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ClientAddresses](
        [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
        [ClientId] UNIQUEIDENTIFIER NOT NULL,
        [Street] NVARCHAR(200) NOT NULL,
        [City] NVARCHAR(100) NOT NULL,
        [State] NVARCHAR(50) NOT NULL,
        [Country] NVARCHAR(50) NOT NULL,
        [PostalCode] NVARCHAR(20) NOT NULL,
        [IsDefault] BIT NOT NULL DEFAULT 0,
        CONSTRAINT [FK_ClientAddresses_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id])
    );
END
GO