IF OBJECTPROPERTY(object_id('SvcUpd_AppServiceRunLog'), N'IsProcedure') = 1
DROP PROCEDURE [SvcUpd_AppServiceRunLog]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Update AppServiceRunLog Information.

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


Create Procedure [dbo].[SvcUpd_AppServiceRunLog]
	(   @p_AppServiceRunLogID     int
	   ,@p_AppServiceRunID        int
	   ,@p_AppServiceQueueID      int
	   ,@p_LogLevel               varchar(50)
	   ,@p_LogMessage             varchar(50)
	   ,@p_LogTime                datetime
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
			UPDATE [AppServiceRunLog]
			
			   SET [AppServiceRunID]   = @p_AppServiceRunID
				  ,[AppServiceQueueID] = @p_AppServiceQueueID
				  ,[LogLevel]          = @p_LogLevel
				  ,[LogMessage]        = @p_LogMessage
				  ,[LogTime]           = @p_LogTime
				  ,[LastModifiedByID]  = @p_LastModifiedByID
				  ,[LastModifiedAt]    = GetDate()--@p_LastModifiedAt
             
             WHERE 1 = 1
              AND AppServiceRunLogID   = @p_AppServiceRunLogID
	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcUpd_AppServiceRunLog' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
