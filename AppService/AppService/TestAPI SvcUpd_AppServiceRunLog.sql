USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppServiceRunLog]
		@p_AppServiceRunLogID = 1,
		@p_AppServiceRunID = 1,
		@p_AppServiceQueueID = 2,
		@p_LogLevel = N'Update',
		@p_LogMessage = N'Update',
		@p_LogTime = N'01-04-2014',
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
