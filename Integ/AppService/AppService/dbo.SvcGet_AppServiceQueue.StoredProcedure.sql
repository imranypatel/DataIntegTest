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
  	  @p_StateID int = -1,
  	  @p_AppServiceQueueID int = -1,
  	  @p_FileName varchar(max) = null,
  	  @tState	  varchar(500) output)  
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	BEGIN TRY
		      SELECT  	      
		           AppServiceQueueID     = AppServiceQueueID
				  ,AppServiceID          = AppServiceID
				  ,MapObjectID           = MapObjectID
				  ,MapObjectTargetID     = MapObjectTargetID
				  ,RetryAttempts         = RetryAttempts
				  ,StateID               = StateID
				  ,StatusID              = StatusID
				  ,[FileName] 
				  ,CreatedByID           = CreatedByID
				  ,CreatedAt             = CreatedAt
				  ,LastModifiedByID      = LastModifiedByID
				  ,LastModifiedAt        = LastModifiedAt
				   
			  FROM AppServiceQueue  
			  WHERE 1=1
			  AND (@p_StateID=-1 OR StateId = @p_StateID)
			  AND (@p_AppServiceQueueID=-1 OR AppServiceQueueID=@p_AppServiceQueueID)
			  AND (@p_FileName is Null or [FileName] =  @p_FileName)
	           Select @tState = 'OK' 
						 + '~' + 'SVC-100001' 
						 + '~' + 'SvcGet_AppServiceQueue'
						 + '~' + 'NONE'
						 + '~' +  'OK'
    END TRY
            
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SVC-100001' 
						 + '~' + 'SvcGet_AppServiceQueue'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
