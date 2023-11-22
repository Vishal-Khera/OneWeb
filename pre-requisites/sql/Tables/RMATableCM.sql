USE [OneWeb_Data]
GO

/****** Object:  Table [dbo].[SXA_RMANumberTracker_CM]    Script Date: 08-12-2021 12:55:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[SXA_RMANumberTracker_CM](
	[SiteName] [varchar](100) NOT NULL,
	[FormId] [varchar](100) NOT NULL,
	[RMANumber] [varchar](100) NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[LastUpdated] [datetime] NOT NULL
) ON [PRIMARY]
GO


