CREATE TABLE [dbo].[OTPMaster] (
    [Id]              INT           IDENTITY (1, 1) NOT NULL,
    [OTP]             VARCHAR (10)  NULL,
    [SendOn]          DATETIME      NULL,
    [OTP_Source]      VARCHAR (20)  NULL,
    [UserEmailId]     VARCHAR (100) NULL,
    [UserPhoneNumber] VARCHAR (15)  NULL,
    [IsUsed]          BIT           NULL,
    CONSTRAINT [PK_OTPMaster] PRIMARY KEY CLUSTERED ([Id] ASC)
);

