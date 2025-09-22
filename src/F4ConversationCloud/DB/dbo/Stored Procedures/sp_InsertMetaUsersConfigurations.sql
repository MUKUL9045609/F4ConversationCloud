CREATE PROCEDURE [dbo].[sp_InsertMetaUsersConfigurations] 
   @WabaId          VARCHAR(50),
  @BusinessId      VARCHAR(100),
  @PhoneNumberId   VARCHAR(100),
  @ClientId        VARCHAR(100),
  @WhatsAppBotName VARCHAR(100),
  @Status          VARCHAR(20),
  @PhoneNumber     VARCHAR(20),
  @AppVersion      VARCHAR(15)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO MetaUsersConfiguration
        (
            WabaId,
            BusinessId,
            PhoneNumberId,
            ClientId,    
            WhatsAppBotName,
            [status],
            PhoneNumber,
            AppVersion,
            CreatedAt
        )
        VALUES 
        (
            @WabaId,
            @BusinessId,
            @PhoneNumberId,
            @ClientId,
           
            @WhatsAppBotName,
            @Status,
            @PhoneNumber,
            @AppVersion,
            GETDATE()
        );

        SELECT 1 AS Status;
    END TRY
    BEGIN CATCH
        SELECT 0 AS Status;
    END CATCH
END;