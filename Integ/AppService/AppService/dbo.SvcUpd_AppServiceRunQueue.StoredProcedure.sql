IF OBJECTPROPERTY(object_id('SvcUpd_AppServiceRunQueue'), N'IsProcedure') = 1
DROP PROCEDURE [SvcUpd_AppServiceRunQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Update AppServiceRunQueue Information.

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


Create Procedure [dbo].[SvcUpd_AppServiceRunQueue]
	(   @p_AppServiceRunQueueID   int
	   ,@p_AppServiceRunID        int
	   ,@p_AppServiceQueueID      int
	   ,@p_StateID                int
	   ,@p_Retry                  int
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
			UPDATE [AppServiceRunQueue]
			
			   SET [AppServiceRunID]   = @p_AppServiceRunID
				  ,[AppServiceQueueID] = @p_AppServiceQueueID
				  ,[StateID]           = @p_StateID
				  ,[Retry]             = @p_Retry
				  ,[StatusID]          = @p_StatusID
				  ,[LastModifiedByID]  = @p_LastModifiedByID
				  ,[LastModifiedAt]    = GetDate()--@p_LastModifiedAt
             
             WHERE 1 = 1
              AND AppServiceRunQueueID   = @p_AppServiceRunQueueID
	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcUpd_AppServiceRunQueue' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
