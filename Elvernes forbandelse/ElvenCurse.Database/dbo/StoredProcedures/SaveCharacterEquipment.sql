CREATE PROCEDURE [dbo].[SaveCharacterEquipment]
	@id int,
	@equipment nvarchar(500)
AS
	update Characters set Equipment = @equipment where Id = @id
