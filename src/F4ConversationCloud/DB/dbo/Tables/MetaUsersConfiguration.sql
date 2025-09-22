CREATE TABLE [dbo].[MetaUsersConfiguration] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [PhoneNumberId]   VARCHAR (200) NULL,
    [WabaId]          VARCHAR (255) NULL,
    [BusinessId]      VARCHAR (200) NULL,
    [ClientId]        INT           NULL,
    [WhatsAppBotName] VARCHAR (200) NULL,
    [status]          VARCHAR (10)  NULL,
    [PhoneNumber]     VARCHAR (50)  NULL,
    [CreatedAt]       DATETIME2 (7) NULL,
    [AppVersion]      VARCHAR (50)  NULL,
    CONSTRAINT [PK_MetaUsersConfiguration] PRIMARY KEY CLUSTERED ([Id] ASC)
);

