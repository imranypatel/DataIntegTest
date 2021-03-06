USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppServiceRunQueue]
		@p_AppServiceRunQueueID = 1,
		@p_AppServiceRunID = 1,
		@p_AppServiceQueueID = 2,
		@p_StateID = 33,
		@p_Retry = 1,
		@p_StatusID = 22,
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
