USE [OneWeb_Data]
GO

/****** Object:  StoredProcedure [dbo].[sp_SXA_FetchAndLockRMANumber_CM]    Script Date: 08-12-2021 12:57:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SXA_FetchAndLockRMANumber_CM]
	-- Add the parameters for the stored procedure here
	@SiteName nvarchar(100), 
	@FormId nvarchar(100)	
AS
BEGIN	
	DECLARE @currentNumber INT
	DECLARE @isLocked BIT
	DECLARE @lastUpdated DATETIME
	SELECT @currentNumber = CAST(RMANumber  AS INT), @isLocked = IsLocked, @lastUpdated = LastUpdated From SXA_RMANumberTracker_CM WHERE (SiteName = @SiteName AND FormId = @FormId)
	PRINT @isLocked
	IF (@isLocked = 1)
	BEGIN
		WHILE (@isLocked = 1)
		BEGIN
				WAITFOR DELAY '000:00:02'
				SELECT @currentNumber = CAST(RMANumber  AS INT), @isLocked = IsLocked, @lastUpdated = LastUpdated From SXA_RMANumberTracker_CM WHERE (SiteName = @SiteName AND FormId = @FormId)
				IF(DATEDIFF(SECOND, @lastUpdated, GETDATE()) > 30)				
				BEGIN
					UPDATE SXA_RMANumberTracker_CM
					SET 					
					IsLocked = 0
					WHERE (SiteName = @SiteName AND FormId = @FormId);				
				END
		END
	END
	SET @currentNumber = @currentNumber + 1
	UPDATE SXA_RMANumberTracker_CM
	SET 
	RMANumber = FORMAT(@currentNumber,'D6'),
	IsLocked = 1,
	LastUpdated = GETDATE()
	WHERE (SiteName = @SiteName AND FormId = @FormId);
	RETURN FORMAT(@currentNumber,'D6')
END
GO


