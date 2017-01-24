CREATE PROCEDURE [dbo].[GetItemsIn]
	@ids nvarchar(500)
AS
	SELECT Id,Category,Type,Name,Description,Imagepath from Items where Id in (@ids)
