CREATE PROCEDURE [dbo].[GetItem]
	@id int
AS
	SELECT Id,Category,Type,Name,Description,Imagepath from Items where Id = @id
