IF OBJECTPROPERTY(object_id('SvcIns_AppServiceRun'), N'IsProcedure') = 1
DROP PROCEDURE [SvcIns_AppServiceRun]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Insert AppServiceRun Information.

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


Create Procedure [dbo].[SvcIns_AppServiceRun]
	(   @p_AppServiceID         int
	   ,@p_StartedAt            datetime
	   --,@p_FinishedAt           datetime
	   ,@p_StatusID             int
	   ,@p_CreatedByID          int
	   --,@p_CreatedAt            datetime
  	   ,@tState	                varchar(500) output)  
		
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ID int =0
	
	BEGIN TRY
		BEGIN 
			 INSERT INTO [AppServiceRun]
			   (
			    [AppServiceID]
			   ,[StartedAt]
			   ,[FinishedAt]
			   ,[StatusID]
			   ,[CreatedByID]
			   ,[CreatedAt]
			   ,[LastModifiedByID]
			   ,[LastModifiedAt]
			   )
		     VALUES
			   (
			    @p_AppServiceID
			   ,@p_StartedAt
			   ,null  --@p_FinishedAt
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
		 From AppServiceRun
		 Where 1=1
		 and AppServiceRunID=@ID
	
    END TRY
	
	BEGIN CATCH
			Select @tState = 'ERR' 
						 + '~' + 'SvcIns_AppServiceRun' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
