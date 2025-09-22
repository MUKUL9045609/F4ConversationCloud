-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================

-- exec [sp_GetCustomerById] 2
Create PROCEDURE [dbo].[sp_GetTimeZones]
 
AS
BEGIN
    SET NOCOUNT ON;

    SELECT name,current_utc_offset
	FROM sys.time_zone_info order by name asc;
END