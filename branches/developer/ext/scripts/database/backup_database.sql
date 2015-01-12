SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


DECLARE @Date AS NVARCHAR(10)
DECLARE @Time AS NVARCHAR(10)
DECLARE @TimeStamp AS NVARCHAR(20)
DECLARE @DatabaseName AS NVARCHAR(255)
DECLARE @DestinationDirectory AS NVARCHAR(255)
DECLARE @DestinationFullPath AS NVARCHAR(255)
DECLARE @Description AS NVARCHAR(255)

SET @Date = CONVERT(NVARCHAR(10), GETDATE(), 126)
SET @Time =
RIGHT('0' + CONVERT(NVARCHAR(2), DATEPART(HH, GETDATE())), 2) + N'-' +
RIGHT('0' + CONVERT(NVARCHAR(2), DATEPART(MI, GETDATE())), 2) + N'-' +
RIGHT('0' + CONVERT(NVARCHAR(2), DATEPART(SS, GETDATE())), 2);
SET @TimeStamp = @Date + N'T' + @Time + N'_'

SET @DatabaseName = N'$(DatabaseName)'
SET @DestinationDirectory = N'$(DestinationDirectory)'
SET @DestinationFullPath = @DestinationDirectory + @TimeStamp + @DatabaseName + N'.bak'
SET @Description = @DatabaseName + N' - complete database backup'

BACKUP DATABASE @DatabaseName TO DISK = @DestinationFullPath WITH FORMAT, INIT, NAME = @Description, SKIP, NOREWIND, NOUNLOAD, STATS = 10

PRINT @DatabaseName
PRINT @DestinationDirectory
PRINT @DestinationFullPath

GO
