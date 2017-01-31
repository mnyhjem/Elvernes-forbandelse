CREATE PROCEDURE [dbo].[SaveCharacterEquipment]
	@id int,
	@equipment nvarchar(4000)
AS
	update Characters set Equipment = @equipment where Id = @id
