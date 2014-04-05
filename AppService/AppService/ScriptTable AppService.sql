CREATE TABLE [dbo].[AppService](
	[AppServiceID] [int] IDENTITY(1,1) NOT NULL,
	[LastRunAt] [datetime] NOT NULL,
	[LastRunStatus] [int] NOT NULL,
	[NextRunAt] [datetime] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedByID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedByID] [int] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL
	PRIMARY KEY ([AppServiceID])
	)
	
	select *
    from dbo.AppService

	
	CREATE TABLE [dbo].[AppServiceRun](
	[AppServiceRunID] [int] IDENTITY(1,1) NOT NULL,
	[AppServiceID] int FOREIGN KEY REFERENCES [AppService](AppServiceID),
	--[AppServiceID] [int] NOT NULL,
	[StartedAt] [datetime] NOT NULL,
	[FinishedAt] [datetime] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedByID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedByID] [int] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL
	PRIMARY KEY ([AppServiceRunID])
	)
	
	select *
    from dbo.AppServiceRun

	
	CREATE TABLE [dbo].[AppServiceQueue](
	[AppServiceQueueID] [int] IDENTITY(1,1) NOT NULL,
	[AppServiceID] int FOREIGN KEY REFERENCES [AppService](AppServiceID),
	--[AppServiceID] [int] NOT NULL,
	[MapObjectID] [int] NOT NULL,
	[MapObjectTargetID] [int] NOT NULL,
	[RetryAttempts] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedByID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedByID] [int] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL
	PRIMARY KEY ([AppServiceQueueID])
	)
	
	select *
    from dbo.AppServiceQueue
	
	
	CREATE TABLE [dbo].[AppServiceRunQueue](
	[AppServiceRunQueueID] [int] IDENTITY(1,1) NOT NULL,
	AppServiceRunID int FOREIGN KEY REFERENCES AppServiceRun(AppServiceRunID),
	AppServiceQueueID int FOREIGN KEY REFERENCES AppServiceQueue(AppServiceQueueID),
	--[AppServiceRunID] [int] NOT NULL,
	--[AppServiceQueueID] [int] NOT NULL,
	[StateID] [int] NOT NULL,
	[Retry] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[CreatedByID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedByID] [int] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL
	PRIMARY KEY ([AppServiceRunQueueID])
	)
	
	select *
    from dbo.AppServiceRunQueue

	
	
	CREATE TABLE [dbo].[AppServiceRunLog](
	[AppServiceRunLogID] [int] IDENTITY(1,1) NOT NULL,
	AppServiceRunID int FOREIGN KEY REFERENCES AppServiceRun(AppServiceRunID),
	AppServiceQueueID int FOREIGN KEY REFERENCES AppServiceQueue(AppServiceQueueID),
	--[AppServiceRunID] [int] NOT NULL,
	--[AppServiceQueueID] [int] NOT NULL,
	[LogLevel] [varchar] (50) NOT NULL,
	[LogMessage] [varchar] (50) NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[CreatedByID] [int] NOT NULL,
	[CreatedAt] [datetime] NOT NULL,
	[LastModifiedByID] [int] NOT NULL,
	[LastModifiedAt] [datetime] NOT NULL
	PRIMARY KEY ([AppServiceRunLogID])
	)
	
	select *
    from dbo.AppServiceRunLog