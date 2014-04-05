USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppService]
		@p_LastRunAt = N'01-04-2014',
		@p_LastRunStatus = 1,
		@p_NextRunAt = N'01-04-2014',
		@p_StatusID = 1,
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
