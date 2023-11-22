USE [OneWeb_Data]
GO

/****** Object:  StoredProcedure [dbo].[sp_SXA_FetchRMANumber_CM]    Script Date: 08-12-2021 12:59:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		SHAMSHER SINGH DHILLON
-- Description:	This SP is to fetch RMA Number
-- =============================================
CREATE PROCEDURE [dbo].[sp_SXA_FetchRMANumber_CM]
	-- Add the parameters for the stored procedure here
	@SiteName nvarchar(100), 
	@FormId nvarchar(100)
AS
BEGIN	
	DECLARE @currentNumber VARCHAR(50)		
		SELECT @currentNumber = RMANumber From SXA_RMANumberTracker_CM WHERE (SiteName = @SiteName AND FormId = @FormId) 
	RETURN @currentNumber
END
GO


