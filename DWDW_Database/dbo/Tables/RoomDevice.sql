CREATE TABLE [dbo].[RoomDevice] (
    [RoomDeviceId] INT      IDENTITY (1, 1) NOT NULL,
    [RoomId]       INT      NULL,
    [DeviceId]     INT      NULL,
    [StartDate]    DATETIME NULL,
    [EndDate]      DATETIME NULL,
    [IsActive]     BIT      NULL,
    PRIMARY KEY CLUSTERED ([RoomDeviceId] ASC),
    CONSTRAINT [FK_RoomDevice_Device] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Device] ([DeviceId]),
    CONSTRAINT [FK_RoomDevice_Room] FOREIGN KEY ([RoomId]) REFERENCES [dbo].[Room] ([RoomId])
);


GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[RoomDevice]([RoomId] ASC, [DeviceId] ASC);

