IF OBJECTPROPERTY(object_id('SvcIns_AppServiceQueue'), N'IsProcedure') = 1
DROP PROCEDURE [SvcIns_AppServiceQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Insert App Service Queue Information.

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


Create Procedure [dbo].[SvcIns_AppServiceQueue]
	(   @p_AppServiceID         int
	   ,@p_MapObjectID          int
	   ,@p_MapObjectTargetID    int
	   ,@p_RetryAttempts        int
	   ,@p_StateID              int
	   ,@p_StatusID             int
	   ,@p_CreatedByID          int
	   --,@p_CreatedAt            datetime
  	   ,@tState	                varchar(500) output)  
		
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRY
		BEGIN 
			 INSERT INTO [AppServiceQueue]
			   (
			    [AppServiceID]
			   ,[MapObjectID]
			   ,[MapObjectTargetID]
			   ,[RetryAttempts]
			   ,[StateID]
			   ,[StatusID]
			   ,[CreatedByID]
			   ,[CreatedAt]
			   ,[LastModifiedByID]
			   ,[LastModifiedAt]
			   )
		     VALUES
			   (
			    @p_AppServiceID
			   ,@p_MapObjectID
			   ,@p_MapObjectTargetID
			   ,@p_RetryAttempts
			   ,@p_StateID
			   ,@p_StatusID
			   ,@p_CreatedByID
			   ,GetDate()--@p_CreatedAt
			     ,@p_CreatedByID
			   ,GetDate()
			   )

	    END

	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcIns_AppServiceQueue' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
