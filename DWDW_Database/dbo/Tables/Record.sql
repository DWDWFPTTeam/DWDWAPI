CREATE TABLE [dbo].[Record] (
    [RecordId]       INT            IDENTITY (1, 1) NOT NULL,
    [UserId]         INT            NULL,
    [DeviceId]       INT            NULL,
    [Type]           INT            NULL,
    [RecordDateTime] DATETIME       NULL,
    [Image]          NVARCHAR (MAX) NULL,
    [Status]         NVARCHAR (MAX) NULL,
    [Comment]        NVARCHAR (MAX) NULL,
    CONSTRAINT [PK__Record__D825195E23AE0B1E] PRIMARY KEY CLUSTERED ([RecordId] ASC),
    CONSTRAINT [FK_Record_Device] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[Device] ([DeviceId]),
    CONSTRAINT [FK_Record_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);












GO
CREATE NONCLUSTERED INDEX [FK]
    ON [dbo].[Record]([DeviceId] ASC);

