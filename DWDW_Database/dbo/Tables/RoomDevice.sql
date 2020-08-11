CREATE TABLE [dbo].[RoomDevice] (
    [roomDeviceId] INT  IDENTITY (1, 1) NOT NULL,
    [roomId]       INT  NULL,
    [deviceId]     INT  NULL,
    [startDate]    DATE NULL,
    [endDate]      DATE NULL,
    [isActive]     BIT  NULL,
    CONSTRAINT [PK__RoomDevi__3384D64BB5FD3955] PRIMARY KEY CLUSTERED ([roomDeviceId] ASC),
    CONSTRAINT [FK_RoomDevice_Device] FOREIGN KEY ([deviceId]) REFERENCES [dbo].[Device] ([deviceId]),
    CONSTRAINT [FK_RoomDevice_Room] FOREIGN KEY ([roomId]) REFERENCES [dbo].[Room] ([roomId])
);




GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[RoomDevice]([RoomId] ASC, [DeviceId] ASC);

