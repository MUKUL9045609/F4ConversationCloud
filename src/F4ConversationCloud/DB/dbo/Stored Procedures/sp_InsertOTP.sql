CREATE PROCEDURE [dbo].[sp_InsertOTP]
    @OTP VARCHAR(10), 
    @OTP_Source VARCHAR(20),
    @UserEmailId VARCHAR(100),
    @UserPhoneNumber VARCHAR(15) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO OTPMaster (OTP, SendOn, OTP_Source, UserEmailId, UserPhoneNumber)
        VALUES (@OTP, GETDATE(), @OTP_Source, @UserEmailId, @UserPhoneNumber);

        SELECT 1 AS Status;
    END TRY
    BEGIN CATCH
        SELECT 
            0 AS Status,
            ERROR_NUMBER() AS ErrorNumber,
            ERROR_MESSAGE() AS ErrorMessage;
    END CATCH
END;