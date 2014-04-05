use [darwin-dev]
Select *
From Status

-- 35-New
-- 36-Closed

---
SELECT TenciaBillableEntityCode = org.TenciaOrgID
FROM MapOrganisation mo
INNER JOIN Organisation org on org.OrganisationID = mo.TargetOrganisationID
WHERE 
	mo.OrganisationTypeID	= 3	-- "Internal with GL"
AND mo.SourceOrganisationID	= 2 	-- "S450 - Spec Property"

---
SELECT *
FROM MapOrganisation mo
INNER JOIN Organisation org on org.OrganisationID = mo.TargetOrganisationID
WHERE 
	mo.OrganisationTypeID	= 2	-- "Vendor"
AND mo.SourceOrganisationID	= 2 	-- "S450 - Spec Property"


/****** Script for SelectTopNRows command from SSMS  ******/
SELECT *
FROM [AppService]

Select *
From AppServiceQueue

Select *
From AppServiceRun

Select *
From AppServiceRunLog

Select *
From AppServiceRunQueue


---
SElect *
From Status

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRun]
		@p_AppServiceID = 3,
		@p_StartedAt = N'01-04-2014',
		@p_FinishedAt = N'01-04-2014',
		@p_StatusID = 1001,  --started
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO
---
DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRunLog]
		@p_AppServiceRunID = 1,
		@p_AppServiceQueueID = 2,
		@p_LogLevel = N'test',
		@p_LogMessage = N'test',
		@p_LogTime = N'01-04-2014',
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value


---
DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRunQueue]
		@p_AppServiceRunID = 1,
		@p_AppServiceQueueID = 2,
		@p_StateID = 11,
		@p_Retry = 0,
		@p_StatusID = 11,
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO

--
DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppServiceRun]
		@p_AppServiceRunID = 1,
		@p_AppServiceID = 3,
		@p_StartedAt = N'01-04-2014',
		@p_FinishedAt = N'01-04-2014',
		@p_StatusID = 33,
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value
