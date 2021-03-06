USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRunQueue]
		@p_AppServiceRunID = 1,
		@p_AppServiceQueueID = 2,
		@p_StateID = 11,
		@p_Retry = 0,
		@p_StatusID = 11,
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
