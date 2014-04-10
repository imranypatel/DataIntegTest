IF OBJECTPROPERTY(object_id('SvcIns_AppServiceRunLog'), N'IsProcedure') = 1
DROP PROCEDURE [SvcIns_AppServiceRunLog]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Insert AppServiceRunLog Information.

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


Create Procedure [dbo].[SvcIns_AppServiceRunLog]
	(   
	   @p_AppServiceID			   int
	   ,@p_AppServiceRunID         int
	   ,@p_AppServiceQueueID       int
	   ,@p_LogLevel                varchar(50)
	   ,@p_LogMessage              varchar(max)
	   ,@p_LogTime                 datetime
	   ,@p_CreatedByID             int
	   --,@p_CreatedAt               datetime
  	   ,@tState	                   varchar(500) output)  
		
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRY
		BEGIN 
			 INSERT INTO [AppServiceRunLog]
			   (
			    [AppServiceID]
			   ,[AppServiceRunID]
			   ,[AppServiceQueueID] 
			   ,[LogLevel]
			   ,[LogMessage]
			   ,[LogTime]
			   ,[CreatedByID]
			   ,[CreatedAt]
			   ,[LastModifiedByID]
			   ,[LastModifiedAt]
			   )
		     VALUES
			   (
			   @p_AppServiceID
			   ,@p_AppServiceRunID
			   ,@p_AppServiceQueueID
			   ,@p_LogLevel
			   ,@p_LogMessage
			   ,@p_LogTime
			   ,@p_CreatedByID
			   ,GetDate()  --@p_CreatedAt
			   ,@p_CreatedByID
			   ,GetDate()  
			   )

	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcIns_AppServiceRunLog' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
