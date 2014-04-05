IF OBJECTPROPERTY(object_id('SvcUpd_AppServiceRun'), N'IsProcedure') = 1
DROP PROCEDURE [SvcUpd_AppServiceRun]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Update AppServiceRun Information.

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


Create Procedure [dbo].[SvcUpd_AppServiceRun]
	(   @p_AppServiceRunID        int
	   --,@p_AppServiceID           int
	   --,@p_StartedAt              datetime
	   ,@p_FinishedAt             datetime
	   ,@p_StatusID               int
	   ,@p_LastModifiedByID       int
	  -- ,@p_LastModifiedAt         datetime
  	   ,@tState	                  varchar(500) output)  
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRY
		BEGIN 
			UPDATE [AppServiceRun]
			
			   SET --[AppServiceID]     = @p_AppServiceID
				  --,[StartedAt]        = @p_StartedAt
				  [FinishedAt]       = @p_FinishedAt
				  ,[StatusID]         = @p_StatusID
				  ,[LastModifiedByID] = @p_LastModifiedByID
				  ,[LastModifiedAt]   = GetDate()--@p_LastModifiedAt
             
             WHERE 1 = 1
              AND AppServiceRunID = @p_AppServiceRunID
	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcUpd_AppServiceRun' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
