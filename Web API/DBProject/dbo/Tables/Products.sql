CREATE TABLE [dbo].[Products] (
    [Id]              INT             IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (MAX)  NOT NULL,
    [Description]     NVARCHAR (MAX)  NOT NULL,
    [Price]           DECIMAL (18, 2) NOT NULL,
    [PictureUrl]      NVARCHAR (MAX)  NOT NULL,
    [Type]            NVARCHAR (MAX)  NOT NULL,
    [Brand]           NVARCHAR (MAX)  NOT NULL,
    [QuantityInStock] INT             NOT NULL,
    [PublicId]        NVARCHAR (MAX)  NOT NULL,
    CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC)
);

