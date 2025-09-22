
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetFilteredUsers] 
	@searchString VARCHAR(100),
	@status BIT,
	@pageNumber INT,
	@pageSize INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @TempResults TABLE (
		[SrNo] INT,
	    [Id] [int],
		[FirstName] [varchar](100),
		[LastName] [varchar](100),
		[Email] [varchar](100),
		[MobileNo] [varchar](100),
		[Role] int,
		[Password] [varchar](100),
		[IPAddress] [varchar](100),
		[Designation] [varchar](100), 
		[IsActive] [bit],
		[CreatedOn] [datetime],
		[UpdatedOn] [datetime],
		[CreatedBy] [int],
		[UpdatedBy] [int],
	    [RoleName] [varchar](100)
	);

	INSERT INTO @TempResults
	SELECT
		ROW_NUMBER() OVER (ORDER BY CASE WHEN U.UpdatedOn > U.CreatedOn THEN U.UpdatedOn ELSE U.CreatedOn END  DESC)
	  ,U.[Id]
      ,U.[FirstName]
      ,U.[LastName]
      ,U.[Email]
      ,U.[MobileNo]
      ,U.[Role]
      ,U.[Password]
      ,U.[IPAddress]
      ,U.[Designation]
      ,U.[IsActive]
      ,U.[CreatedOn]
      ,U.[UpdatedOn]
      ,U.[CreatedBy]
      ,U.[UpdatedBy]
	  ,RL.[Name] as RoleName
	FROM [dbo].[SuperAdminUsers] U
	INNER JOIN [dbo].[SuperAdminRoles] AS RL ON RL.Id = U.Role
	WHERE U.[IsActive]= @status
	AND (U.[FirstName] like '%'+@searchString+'%' OR U.[LastName] like '%'+@searchString+'%')

	select  Top(@pageSize) * from @TempResults
	WHERE [SrNo] > @pageSize * (@pageNumber - 1)
	ORDER BY CASE WHEN UpdatedOn > CreatedOn THEN UpdatedOn
			 ELSE CreatedOn END DESC
END