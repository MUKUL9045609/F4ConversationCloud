
Create PROCEDURE [dbo].[sp_UpdateClientPassword] 
	@id int,
	@password VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE client_info
	SET [Password] = @password
	WHERE [Id] = @id

	SELECT @id
END