USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRun]
		@p_AppServiceID = 3,
		@p_StartedAt = N'01-04-2014',
		@p_FinishedAt = N'01-04-2014',
		@p_StatusID = 11,
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
