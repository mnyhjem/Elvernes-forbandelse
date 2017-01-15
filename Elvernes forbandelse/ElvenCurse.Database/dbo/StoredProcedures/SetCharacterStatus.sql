CREATE PROCEDURE [dbo].[SetCharacterStatus]
	@characterId int,
	@isAlive bit
AS
	update Characters set IsAlive = @isalive where Id = @characterId
RETURN 0
