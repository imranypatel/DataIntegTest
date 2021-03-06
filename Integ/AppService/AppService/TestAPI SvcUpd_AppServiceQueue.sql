USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppServiceQueue]
		@p_AppServiceQueueID = 2,
		@p_AppServiceID = 3,
		@p_MapObjectID = 22,
		@p_MapObjectTargetID = 33,
		@p_RetryAttempts = 11,
		@p_StateID = 2,
		@p_StatusID = 44,
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
