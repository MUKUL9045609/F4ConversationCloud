
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[sp_GetFilteredClientUsers] 
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
		[PhoneNumber] [varchar](100),
		[Role] [varchar](100),
		[Password] [varchar](100),
		[IsActive] [bit],
		[CreatedOn] [datetime]
	);

	INSERT INTO @TempResults
	SELECT
		ROW_NUMBER() OVER (ORDER BY U.CreatedOn DESC)
	  ,U.[Id]
      ,U.[FirstName]
      ,U.[LastName]
      ,U.[Email]
      ,U.[PhoneNumber]
      ,U.[Role]
      ,U.[password]
      ,U.[IsActive]
      ,U.[CreatedOn]
	FROM [dbo].[client_info] U
	WHERE (U.[FirstName] like '%'+@searchString+'%' OR U.[LastName] like '%'+@searchString+'%')

	select  Top(@pageSize) * from @TempResults
	WHERE [SrNo] > @pageSize * (@pageNumber - 1)
	ORDER BY CreatedOn DESC
END