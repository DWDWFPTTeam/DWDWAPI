CREATE TABLE [dbo].[Role] (
    [roleId]   INT           IDENTITY (1, 1) NOT NULL,
    [roleName] NVARCHAR (50) NULL,
    [isActive] BIT           NULL,
    PRIMARY KEY CLUSTERED ([roleId] ASC)
);

