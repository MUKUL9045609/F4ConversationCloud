CREATE TABLE [dbo].[client_info] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]      VARCHAR (100)  NULL,
    [LastName]       VARCHAR (100)  NULL,
    [Email]          VARCHAR (100)  NULL,
    [password]       VARCHAR (100)  NULL,
    [PhoneNumber]    VARCHAR (15)   NULL,
    [Address]        NVARCHAR (MAX) NULL,
    [Country]        VARCHAR (100)  NULL,
    [Role]           NVARCHAR (50)  NULL,
    [isActive]       BIT            NULL,
    [CreatedOn]      DATETIME       NULL,
    [ClientTimeZone] VARCHAR (200)  NULL,
    [Stage]          INT            NULL,
    CONSTRAINT [PK_client_info] PRIMARY KEY CLUSTERED ([Id] ASC)
);

