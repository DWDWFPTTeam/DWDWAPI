CREATE TABLE [dbo].[Location] (
    [LocationId]   INT           IDENTITY (1, 1) NOT NULL,
    [LocationCode] NVARCHAR (50) NULL,
    [IsActive]     BIT           NULL,
    PRIMARY KEY CLUSTERED ([LocationId] ASC)
);

