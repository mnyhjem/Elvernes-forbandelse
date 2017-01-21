CREATE PROCEDURE [dbo].[SetCharacterOnline]
	@characterId int,
	@userId nvarchar(50)
AS
	
update Characters set IsOnline = 0 where UserId = @userId;
update Characters set IsOnline = 1 where UserId = @userId and Id = @characterId;
