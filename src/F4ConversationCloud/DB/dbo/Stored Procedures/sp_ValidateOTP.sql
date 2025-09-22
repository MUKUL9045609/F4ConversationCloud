-- exec sp_ValidateOTP '710301',null,'ashwinchandode199@gmail.com'
-- exec sp_ValidateOTP '451596', null, 'ashwinchandode199@gmail.com'

CREATE PROCEDURE [dbo].[sp_ValidateOTP]
    @OTP NVARCHAR(10),
    @UserPhoneNumber VARCHAR(10) = NULL,
    @UserEmailId VARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;


        BEGIN TRANSACTION;

        DECLARE @IsValid INT = 0;

        -- Validate OTP
        SELECT TOP 1 @IsValid = 1
        FROM OTPMaster
        WHERE OTP = @OTP
          AND (
                (@UserPhoneNumber IS NOT NULL AND UserPhoneNumber = @UserPhoneNumber)
                OR (@UserEmailId IS NOT NULL AND UserEmailId = @UserEmailId)
              )
          AND ISNULL(IsUsed, 0) = 0
          AND DATEDIFF(MINUTE, SendOn, GETDATE()) <= 5;

        IF @IsValid = 1
        BEGIN
            UPDATE OTPMaster
            SET IsUsed = 1
            WHERE OTP = @OTP
              AND (
                    (@UserPhoneNumber IS NOT NULL AND UserPhoneNumber = @UserPhoneNumber)
                    OR (@UserEmailId IS NOT NULL AND UserEmailId = @UserEmailId)
                  )
              AND ISNULL(IsUsed, 0) = 0;

            SELECT @IsValid AS status;
        END
        ELSE
        BEGIN
            SELECT @IsValid AS status;
        END

        COMMIT TRANSACTION;
   
END;