USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppServiceRun]
		@p_AppServiceRunID = 1,
		@p_AppServiceID = 3,
		@p_StartedAt = N'01-04-2014',
		@p_FinishedAt = N'01-04-2014',
		@p_StatusID = 33,
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
