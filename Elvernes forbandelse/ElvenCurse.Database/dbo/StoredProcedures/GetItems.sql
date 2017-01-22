CREATE PROCEDURE [dbo].[GetItems]
	
AS
	SELECT Id,Category,Type,Name,Description,Imagepath from Items order by Id
