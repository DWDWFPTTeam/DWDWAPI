CREATE TABLE [dbo].[Arrangement] (
    [ArrangementId] INT      IDENTITY (1, 1) NOT NULL,
    [UserId]        INT      NULL,
    [LocationId]    INT      NULL,
    [StartDate]     DATETIME NULL,
    [EndDate]       DATETIME NULL,
    [IsActive]      BIT      NULL,
    CONSTRAINT [PK__UserLoca__1C86249285DF2F14] PRIMARY KEY CLUSTERED ([ArrangementId] ASC),
    CONSTRAINT [FK_UserLocation_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId]),
    CONSTRAINT [FK_UserLocation_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[Arrangement]([UserId] ASC, [LocationId] ASC);

