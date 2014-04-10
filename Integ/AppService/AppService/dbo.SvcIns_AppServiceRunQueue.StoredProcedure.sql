IF OBJECTPROPERTY(object_id('SvcIns_AppServiceRunQueue'), N'IsProcedure') = 1
DROP PROCEDURE [SvcIns_AppServiceRunQueue]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Insert AppServiceRunQueue Information.

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


Create Procedure [dbo].[SvcIns_AppServiceRunQueue]
	(   @p_AppServiceRunID         int
	   ,@p_AppServiceQueueID       int
	   ,@p_StateID                 int
	   ,@p_Retry                   int
	   ,@p_StatusID                int
	   ,@p_CreatedByID             int
	   --,@p_CreatedAt               datetime
  	   ,@tState	                   varchar(500) output)  
		
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID int =0
	
	BEGIN TRY
		BEGIN 
			 INSERT INTO [AppServiceRunQueue]
			   (
			    [AppServiceRunID]
			   ,[AppServiceQueueID] 
			   ,[StateID]
			   ,[Retry]
			   ,[StatusID]
			   ,[CreatedByID]
			   ,[CreatedAt]
			   ,[LastModifiedByID]
			   ,[LastModifiedAt]
			   )
		     VALUES
			   (
			    @p_AppServiceRunID
			   ,@p_AppServiceQueueID
			   ,@p_StateID
			   ,@p_Retry
			   ,@p_StatusID
			   ,@p_CreatedByID
			   ,GetDate()--@p_CreatedAt
               ,@p_CreatedByID
			   ,GetDate()
			   )

	    END
		
		 SELECT  @ID = @@IDENTITY 
		 Select @tState = 'OK' 
							 + '~' + 'SVC-100001' 
							 + '~' + 'NONE'
							 + '~' + Cast(@ID as varchar)
							 + '~' + 'Service run added successfully'
		 Select *
		 From [AppServiceRunQueue]
		 Where 1=1
		 and AppServiceRunQueueID=@ID
	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcIns_AppServiceRunQueue' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
