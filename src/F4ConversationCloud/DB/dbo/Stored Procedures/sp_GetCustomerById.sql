-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCustomerById]
   @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id AS UserId,
        FirstName,
        LastName,
        Email,
        PhoneNumber,
        Address,
        Country,
        clientTimeZone AS TimeZone,
		Stage,
        Role
    FROM client_info
    WHERE Id = @UserId;
END