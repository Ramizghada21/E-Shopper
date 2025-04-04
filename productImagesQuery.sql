CREATE TABLE [dbo].[ProductImages] (
    [ImageId]      INT            IDENTITY (1,1) NOT NULL,
    [ImageUrl]     NVARCHAR(MAX)  NOT NULL,
    [ProductId]    INT            NOT NULL,  -- Fixed column name
    [DefaultImage] BIT            NOT NULL,

    -- Primary Key Constraint
    CONSTRAINT [PK_ProductImages] PRIMARY KEY CLUSTERED ([ImageId] ASC),

    -- Foreign Key Constraint
    CONSTRAINT [FK_ProductImages_Product] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId]) ON DELETE CASCADE
);
