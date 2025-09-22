
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_GetUserCount] 
	@searchString VARCHAR(100),
	@status BIT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(1)
	FROM [dbo].[SuperAdminUsers]
	WHERE [IsActive]= @status
	AND ([FirstName] like '%'+@searchString+'%' or [LastName] like '%'+@searchString+'%')
END