CREATE TABLE [dbo].[SuperAdminRoles] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NULL,
    [IsActive] BIT           DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

