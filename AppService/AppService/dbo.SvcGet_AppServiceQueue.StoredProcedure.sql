IF OBJECTPROPERTY(object_id('SvcGet_AppServiceQueue'), N'IsProcedure') = 1
DROP PROCEDURE [SvcGet_AppServiceQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to reterive AppServiceQueue Information.

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


Create Procedure [dbo].[SvcGet_AppServiceQueue]
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
		      
		           AppServiceQueueID     = AppServiceQueueID
				  ,AppServiceID          = AppServiceID
				  ,MapObjectID           = MapObjectID
				  ,MapObjectTargetID     = MapObjectTargetID
				  ,RetryAttempts         = RetryAttempts
				  ,StateID               = StateID
				  ,StatusID              = StatusID
				  ,CreatedByID           = CreatedByID
				  ,CreatedAt             = CreatedAt
				  ,LastModifiedByID      = LastModifiedByID
				  ,LastModifiedAt        = LastModifiedAt
				   
			  FROM AppServiceQueue  

	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcGet_AppServiceQueue' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
