CREATE TABLE [dbo].[User] (
    [UserId]      INT            IDENTITY (1, 1) NOT NULL,
    [UserName]    NVARCHAR (MAX) NULL,
    [Password]    NVARCHAR (MAX) NULL,
    [FullName]    NVARCHAR (MAX) NULL,
    [Phone]       NVARCHAR (MAX) NULL,
    [DateOfBirth] DATETIME       NULL,
    [Gender]      INT            NULL,
    [DeviceToken] NVARCHAR (MAX) NULL,
    [RoleId]      INT            NULL,
    [IsActive]    BIT            NULL,
    CONSTRAINT [PK__User__CB9A1CFF151180C8] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_User_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([roleId])
);






GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[User]([RoleId] ASC);

