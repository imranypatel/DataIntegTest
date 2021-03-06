IF OBJECTPROPERTY(object_id('SvcUpd_AppService'), N'IsProcedure') = 1
DROP PROCEDURE [SvcUpd_AppService]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Update App Service Information.

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


Create Procedure [dbo].[SvcUpd_AppService]
	(   @p_AppServiceID           int
	   ,@p_LastRunAt              datetime
	   ,@p_LastRunStatus          int
	   ,@p_NextRunAt              datetime
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
			UPDATE [AppService]
			
             SET   [LastRunAt]        = @p_LastRunAt
				  ,[LastRunStatus]    = @p_LastRunStatus
				  ,[NextRunAt]        = @p_NextRunAt
				  ,[StatusID]         = @p_StatusID
				  ,[LastModifiedByID] = @p_LastModifiedByID
				  ,[LastModifiedAt]   = GetDate()--@p_LastModifiedAt
             
             WHERE 1 = 1
              AND AppServiceID = @p_AppServiceID
	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcUpd_AppService' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
