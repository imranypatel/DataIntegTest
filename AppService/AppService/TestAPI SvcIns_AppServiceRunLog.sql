USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRunLog]
		@p_AppServiceRunID = 1,
		@p_AppServiceQueueID = 2,
		@p_LogLevel = N'test',
		@p_LogMessage = N'test',
		@p_LogTime = N'01-04-2014',
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
