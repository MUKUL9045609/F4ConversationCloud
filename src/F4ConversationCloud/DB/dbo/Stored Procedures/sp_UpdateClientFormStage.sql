
CREATE PROCEDURE [dbo].[sp_UpdateClientFormStage]
	@UserId int,
	@stageId int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE client_info
	SET Stage = @stageId
	WHERE [Id] = @UserId

	SELECT @UserId
END