IF OBJECTPROPERTY(object_id('SvcGet_AppServiceRunQueue'), N'IsProcedure') = 1
DROP PROCEDURE [SvcGet_AppServiceRunQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to reterive AppServiceRunQueue Information.

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


Create Procedure [dbo].[SvcGet_AppServiceRunQueue]
  	 (
  	  @tState	  varchar(500) output)  
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRY
		BEGIN 
		      SELECT  
		           AppServiceRunQueueID  = AppServiceRunQueueID
		          ,AppServiceRunID       = AppServiceRunID
		          ,AppServiceQueueID     = AppServiceQueueID
				  ,StateID               = StateID
				  ,Retry                 = Retry
				  ,StatusID              = StatusID
				  ,CreatedByID           = CreatedByID
				  ,CreatedAt             = CreatedAt
				  ,LastModifiedByID      = LastModifiedByID
				  ,LastModifiedAt        = LastModifiedAt
				   
			  FROM [AppServiceRunQueue]  

	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcGet_AppServiceRunQueue' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
