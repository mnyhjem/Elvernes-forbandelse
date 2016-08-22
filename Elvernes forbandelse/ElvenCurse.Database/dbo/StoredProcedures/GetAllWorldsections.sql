CREATE PROCEDURE [dbo].[GetAllWorldsections]
	
AS
SELECT Id, Mapchange_Down, Mapchange_Left, Mapchange_Right, Mapchange_Up, Name, [Json] from Worldsections order by id