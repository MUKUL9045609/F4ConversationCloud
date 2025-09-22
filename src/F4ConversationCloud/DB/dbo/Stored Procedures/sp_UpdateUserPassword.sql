
CREATE PROCEDURE [dbo].[sp_UpdateUserPassword] 
	@id int,
	@password VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE SuperAdminUsers
	SET [Password] = @password
	WHERE [Id] = @id

	SELECT @id
END