CREATE TABLE [dbo].[Room] (
    [RoomId]     INT           IDENTITY (1, 1) NOT NULL,
    [RoomCode]   NVARCHAR (50) NULL,
    [LocationId] INT           NULL,
    [IsActive]   BIT           NULL,
    PRIMARY KEY CLUSTERED ([RoomId] ASC),
    CONSTRAINT [FK_Room_Location] FOREIGN KEY ([LocationId]) REFERENCES [dbo].[Location] ([LocationId])
);


GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[Room]([LocationId] ASC);

