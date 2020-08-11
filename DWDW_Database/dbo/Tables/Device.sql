CREATE TABLE [dbo].[Device] (
    [deviceId]   INT           IDENTITY (1, 1) NOT NULL,
    [deviceCode] NVARCHAR (50) NULL,
    [isActive]   BIT           NULL,
    PRIMARY KEY CLUSTERED ([deviceId] ASC)
);



