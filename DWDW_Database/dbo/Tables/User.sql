CREATE TABLE [dbo].[User] (
    [UserId]      INT            IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (100) NULL,
    [Password]    NVARCHAR (100) NULL,
    [Phone]       INT            NULL,
    [DateOfBirth] DATETIME       NULL,
    [Gender]      INT            NULL,
    [DeviceToken] NVARCHAR (300) NULL,
    [RoleId]      INT            NULL,
    [IsActive]    BIT            NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([roleId])
);


GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[User]([RoleId] ASC);

