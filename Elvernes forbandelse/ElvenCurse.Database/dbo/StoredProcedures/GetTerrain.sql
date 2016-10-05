CREATE PROCEDURE [dbo].[GetTerrain]
	@id int
AS
	SELECT Id, Filename, Data from Terrains where id = @id

