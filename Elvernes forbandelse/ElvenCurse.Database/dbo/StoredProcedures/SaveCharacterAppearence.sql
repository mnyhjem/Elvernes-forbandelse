CREATE PROCEDURE [dbo].[SaveCharacterAppearence]
	@id int,
	@appearance nvarchar(4000)
AS
	update Characters set Appearance = @appearance where Id = @id
