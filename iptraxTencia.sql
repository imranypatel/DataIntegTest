use [darwin-dev]
Select *
From Status

-- 35-New
-- 36-Closed

SElect *
From MapObject
-- 9 Org
/*
100 TOBESENT-PENDING
105 STAGED

200 STAGED-PENDING
205 SENT
210 STAGED-MISSING

300 SENT-PENDING
305 SENT-REJECTED
310 SENT-SUCCESS
315 SENT-MISSING


*/

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
ORder by 1 desc

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

---
Delete AppServiceRunLog
Delete AppServiceRunQueue
Delete AppServiceRun
delete AppServiceQueue

SElect *
From AppServiceRunQueue
ORder by 1 desc

Select *
From AppServiceQueue
Where 1=1
order by 1 desc

DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceQueue]
		@p_AppServiceID = 3,
		@p_MapObjectID = 9,  -- Org
		@p_MapObjectTargetID = 9990,  -- OrgId
		@p_RetryAttempts = 0,
		@p_StateID = 100,    -- 100-ToBeSent
		@p_StatusID = 35, -- new
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO




---
DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcGet_AppServiceQueue]
		@p_StateID = 100,    -- 100-ToBeSent
		@p_AppServiceQueueID = -1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

----
DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcUpd_AppServiceQueue]
		@p_AppServiceQueueID = 3,
		--@p_AppServiceID = 3,
		--@p_MapObjectID = 22,
		--@p_MapObjectTargetID = 33,
		@p_FileName = 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaabc.txt',
		@p_RetryAttempts = 11,
		@p_StateID = 105,    -- 105 -- STAGED
		@p_StatusID = 36,  -- 36--CLOSED
		@p_LastModifiedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

Select *
From AppServiceQueue
ORder by 1 desc
-----
Select *
From AppServiceRun
ORder by 1 desc

Select *
From AppServiceQueue
ORder by 1 desc


DECLARE	@return_value int,
		@tState varchar(500)

EXEC	@return_value = [dbo].[SvcIns_AppServiceRunQueue]
		@p_AppServiceRunID = 13,
		@p_AppServiceQueueID = 4,
		@p_StateID = 100,
		@p_Retry = 0,
		@p_StatusID = 36,
		@p_CreatedByID = 1,
		@tState = @tState OUTPUT

SELECT	@tState as N'@tState'

SELECT	'Return Value' = @return_value

GO

Select *
From AppServiceRunQueue
ORder by 1 desc

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
