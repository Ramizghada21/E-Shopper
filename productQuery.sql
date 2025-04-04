CREATE TABLE [dbo].[Product] (
    [ProductId]             INT             IDENTITY (1,1) NOT NULL,
    [ProductName]           NVARCHAR(MAX)   NOT NULL,
    [ShortDescription]      NVARCHAR(MAX)   NULL,
    [LongDescription]       NVARCHAR(MAX)   NULL,
    [AdditionalDescription] NVARCHAR(MAX)   NULL,
    [Price]                 DECIMAL(18,2)   NOT NULL,
    [Quantity]              INT             NOT NULL,
    [Size]                  NVARCHAR(MAX)   NULL,
    [Color]                 NVARCHAR(MAX)   NULL,
    [CompanyName]           NVARCHAR(MAX)   NULL,
    [CategoryId]            INT             NOT NULL,  -- Added INT type
    [SubCategoryId]         INT             NOT NULL,
    [Sold]                  INT             NULL,
    [IsCustomized]          BIT             NOT NULL,
    [IsActive]              BIT             NOT NULL,
    [createdDate]           DATETIME        NOT NULL,
    
    -- Primary Key Constraint
    CONSTRAINT [PK_Product] PRIMARY KEY ([ProductId]),

    -- Foreign Key Constraints
    CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId]) ON DELETE CASCADE,
);
