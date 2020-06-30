CREATE TABLE [dbo].[Device] (
    [DeviceId]     INT           IDENTITY (1, 1) NOT NULL,
    [DeviceCode]   NVARCHAR (50) NULL,
    [DeviceStatus] INT           NULL,
    [IsActive]     BIT           NULL,
    PRIMARY KEY CLUSTERED ([DeviceId] ASC)
);

