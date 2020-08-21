CREATE TABLE [dbo].[Shift] (
    [ShiftId]       INT  IDENTITY (1, 1) NOT NULL,
    [ArrangementId] INT  NULL,
    [Date]          DATE NULL,
    [RoomId]        INT  NULL,
    [IsActive]      BIT  NULL,
    CONSTRAINT [PK__Shift__F2F06B029921998A] PRIMARY KEY CLUSTERED ([ShiftId] ASC),
    CONSTRAINT [FK_Shift_Room] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([RoomId]),
    CONSTRAINT [FK_Shift_UserLocation] FOREIGN KEY ([ArrangementId]) REFERENCES [dbo].[Arrangement] ([ArrangementId])
);










GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[Shift]([ArrangementId] ASC, [RoomId] ASC);

