USE [Darwin-Dev]
GO

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceQueue]
		@p_AppServiceID = 3,
		@p_MapObjectID = 7,
		@p_MapObjectTargetID = 13,
		@p_RetryAttempts = 2,
		@p_StateID = 0,
		@p_StatusID = 11,
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
