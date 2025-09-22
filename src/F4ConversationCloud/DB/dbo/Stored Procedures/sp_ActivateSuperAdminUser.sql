Create PROCEDURE [dbo].[sp_ActivateSuperAdminUser]
    @Id INT
AS
BEGIN
   SET NOCOUNT ON;
   IF(EXISTS(SELECT 1 FROM [dbo].SuperAdminUsers WHERE [Id] = @id))
	BEGIN
		UPDATE [dbo].[SuperAdminUsers]
		   SET [IsActive] = 1,
		     [UpdatedOn] = dbo.fn_getCurrentISTTime()
		 WHERE [Id] = @Id

		 SELECT 1
	END
	ELSE
	BEGIN
		SELECT 0
	END
END