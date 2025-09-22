CREATE function [dbo].[fn_getCurrentISTTime]()
returns datetime
as
begin
	declare @currentTime datetime = GETDATE()
	declare @timeZone nvarchar(max)

	select @timeZone = CURRENT_TIMEZONE()

	if(@timeZone='(UTC) Coordinated Universal Time')
	begin
		select @currentTime = DATEADD(MINUTE,30 ,DATEADD(HOUR,5,GETDATE()))
	end

	return @currentTime
end