CREATE TABLE [dbo].[Shift] (
    [ShiftId]       INT      IDENTITY (1, 1) NOT NULL,
    [ArrangementId] INT      NULL,
    [Date]          DATETIME NULL,
    [RoomId]        INT      NULL,
    [ShiftType]     INT      NULL,
    [IsActive]      BIT      NULL,
    PRIMARY KEY CLUSTERED ([ShiftId] ASC),
    CONSTRAINT [FK_Shift_Room] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([RoomId]),
    CONSTRAINT [FK_Shift_UserLocation] FOREIGN KEY ([ArrangementId]) REFERENCES [dbo].[Arrangement] ([ArrangementId])
);


GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[Shift]([ArrangementId] ASC, [RoomId] ASC);

