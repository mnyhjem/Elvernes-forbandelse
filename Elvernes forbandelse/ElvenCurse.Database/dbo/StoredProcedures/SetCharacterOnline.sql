CREATE PROCEDURE [dbo].[SetCharacterOnline]
	@characterId int,
	@userId nvarchar(50)
AS
	
update Characters set IsOnline = 0 where userId = @userId;
update Characters set IsOnline = 1 where userId = @userId and id = @characterId;
