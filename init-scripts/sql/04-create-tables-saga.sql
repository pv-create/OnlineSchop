USE SagaDb;
GO

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderStates]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[OrderStates](
        [CorrelationId] UNIQUEIDENTIFIER PRIMARY KEY,
        [CurrentState] NVARCHAR(64) NOT NULL,
        [OrderId] UNIQUEIDENTIFIER NOT NULL,
        [ClientId] UNIQUEIDENTIFIER NOT NULL,
        [TotalAmount] DECIMAL(18,2) NOT NULL,
        [OrderDate] DATETIME2 NOT NULL,
        [IsClientValidated] BIT NULL,
        [IsOrderValidated] BIT NULL,
        [FailureReason] NVARCHAR(1024) NULL,
        [Created] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        [Updated] DATETIME2 NULL
    );
END
GO