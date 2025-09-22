
CREATE PROCEDURE [dbo].[sp_CheckUserExists] 
	@email VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT U.[Id],
        U.[FirstName],
        U.[LastName],
        U.[Email],
        U.[Password],
        U.[IsActive],
		U.[Role],
        RL.[Name] AS [RoleName],
        U.[MobileNo]
	FROM [dbo].[SuperAdminUsers] AS U
    INNER JOIN [dbo].[SuperAdminRoles] AS RL ON U.Role = RL.Id
    WHERE U.[Email] = @email
END