CREATE PROCEDURE [dbo].[GetWorldsection]
	@id int
AS
	SELECT Id, Mapchange_Down, Mapchange_Left, Mapchange_Right, Mapchange_Up, Name, [Json] from Worldsections where id = @id

