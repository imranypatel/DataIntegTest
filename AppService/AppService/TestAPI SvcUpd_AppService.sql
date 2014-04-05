USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppService]
		@p_AppServiceID = 3,
		@p_LastRunAt = N'01-04-2014',
		@p_LastRunStatus = 33,
		@p_NextRunAt = N'01-04-2014',
		@p_StatusID = 22,
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
