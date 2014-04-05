IF OBJECTPROPERTY(object_id('SvcUpd_AppServiceQueue'), N'IsProcedure') = 1
DROP PROCEDURE [SvcUpd_AppServiceQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Update AppServiceQueue Information.

SAJ = Muhammad Salman Ajmeri
IP = Imran Patel
MM = Muneeb Mustafa

<ChangeLog>
Change No:   Date:          Author:			Description:                          
_________    ___________    _______		_____________________________________
   001       01-04-2014       MM			 Created.      
                      
</ChangeLog>
*******************************************************************************
*/


Create Procedure [dbo].[SvcUpd_AppServiceQueue]
	(   @p_AppServiceQueueID      int
	   ,@p_AppServiceID           int
	   ,@p_MapObjectID            int
	   ,@p_MapObjectTargetID      int
	   ,@p_RetryAttempts          int
	   ,@p_StateID                int
	   ,@p_StatusID               int
	   ,@p_LastModifiedByID       int
	   --,@p_LastModifiedAt         datetime
  	   ,@tState	                  varchar(500) output)  
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRY
		BEGIN 
			UPDATE [AppServiceQueue]
			
			   SET [AppServiceID]      = @p_AppServiceID
				  ,[MapObjectID]       = @p_MapObjectID
				  ,[MapObjectTargetID] = @p_MapObjectTargetID
				  ,[RetryAttempts]     = @p_RetryAttempts
				  ,[StateID]           = @p_StateID
				  ,[StatusID]          = @p_StatusID
				  ,[LastModifiedByID]  = @p_LastModifiedByID
				  ,[LastModifiedAt]    = GetDate()--@p_LastModifiedAt
             
             WHERE 1 = 1
              AND AppServiceQueueID = @p_AppServiceQueueID
	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcUpd_AppServiceQueue' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
