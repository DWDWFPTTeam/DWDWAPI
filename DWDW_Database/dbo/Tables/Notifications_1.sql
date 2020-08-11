CREATE TABLE [dbo].[Notifications] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [UserId]         INT            NULL,
    [MessageTitle]   NVARCHAR (MAX) NULL,
    [MessageTime]    DATETIME       NULL,
    [MessageContent] NVARCHAR (MAX) NULL,
    [Type]           INT            NULL,
    [IsRead]         BIT            NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notifications_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);





