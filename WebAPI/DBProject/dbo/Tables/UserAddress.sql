CREATE TABLE [dbo].[UserAddress] (
    [Id] INT NOT NULL,
    CONSTRAINT [PK_UserAddress] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserAddress_Users_Id] FOREIGN KEY ([Id]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);

