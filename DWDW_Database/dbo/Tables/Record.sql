CREATE TABLE [dbo].[Record] (
    [RecordId]       INT            IDENTITY (1, 1) NOT NULL,
    [DeviceId]       INT            NULL,
    [RecordDateTime] DATETIME       NULL,
    [Image]          NVARCHAR (255) NULL,
    PRIMARY KEY CLUSTERED ([RecordId] ASC),
    CONSTRAINT [FK_Record_Device] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Device] ([DeviceId])
);






GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[Record]([DeviceId] ASC);

