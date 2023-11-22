USE [OneWeb_Data]
GO

/****** Object:  StoredProcedure [dbo].[sp_SXA_ReleaseRMANumber_CM]    Script Date: 08-12-2021 12:59:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SXA_ReleaseRMANumber_CM]
	-- Add the parameters for the stored procedure here
	@SiteName nvarchar(100), 
	@FormId nvarchar(100)	
AS
BEGIN			
	DECLARE @currentNumber VARCHAR(50)
	UPDATE SXA_RMANumberTracker_CM
	SET 	
		IsLocked = 0
		WHERE (SiteName = @SiteName AND FormId = @FormId);	
	SELECT @currentNumber = RMANumber FROM SXA_RMANumberTracker_CM WHERE (SiteName = @SiteName AND FormId = @FormId);	

	RETURN @currentNumber
END
GO


