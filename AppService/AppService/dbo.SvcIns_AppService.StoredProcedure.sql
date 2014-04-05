IF OBJECTPROPERTY(object_id('SvcIns_AppService'), N'IsProcedure') = 1
DROP PROCEDURE [SvcIns_AppService]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*
*******************************************************************************

-- Description / Purpose: -- Create procedure to Insert App Service Information.

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


Create Procedure [dbo].[SvcIns_AppService]
	(   @p_LastRunAt         datetime
	   ,@p_LastRunStatus     int
	   ,@p_NextRunAt         datetime
	   ,@p_StatusID          int
	   ,@p_CreatedByID       int
	   --,@p_CreatedAt         datetime
  	   ,@tState	             varchar(500) output)  
AS 


BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
	BEGIN TRY
		BEGIN 
			 INSERT INTO [AppService]
			   (
			    [LastRunAt]
			   ,[LastRunStatus]
			   ,[NextRunAt]
			   ,[StatusID]
			   ,[CreatedByID]
			   ,[CreatedAt]
			   ,[LastModifiedByID]
			   ,[LastModifiedAt]
			   )
		     VALUES
			   (
			    @p_LastRunAt
			   ,@p_LastRunStatus
			   ,@p_NextRunAt
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
						 + '~' + 'SvcIns_AppService' 
						 + '~' + 'NONE'
						 + '~' + 'NONE'
						 + '~' +  ERROR_MESSAGE()   
	END CATCH
	

END
