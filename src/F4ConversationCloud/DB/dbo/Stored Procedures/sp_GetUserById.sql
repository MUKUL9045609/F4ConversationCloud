
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_GetUserById] 
	@id int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [Id]
		,[FirstName]
		,[LastName]
		,[Email]
		,[MobileNo]
		,[Role]
		,[Password]
		,[IPAddress]
		,[Designation]
		,[IsActive]
		,[CreatedOn]
		,[UpdatedOn]
	FROM [dbo].[SuperAdminUsers]
	WHERE [Id] = @id
END