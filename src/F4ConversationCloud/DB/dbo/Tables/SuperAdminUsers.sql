CREATE TABLE [dbo].[SuperAdminUsers] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Email]       NVARCHAR (50) NULL,
    [Password]    NVARCHAR (50) NULL,
    [Role]        INT           NULL,
    [IsActive]    BIT           DEFAULT ((1)) NULL,
    [CreatedOn]   DATETIME      NULL,
    [CreatedBy]   NVARCHAR (50) NULL,
    [UpdatedOn]   DATETIME      NULL,
    [UpdatedBy]   NVARCHAR (50) NULL,
    [FirstName]   NVARCHAR (50) NULL,
    [LastName]    NVARCHAR (50) NULL,
    [IPAddress]   NVARCHAR (50) NULL,
    [MobileNo]    NVARCHAR (50) NULL,
    [Designation] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

