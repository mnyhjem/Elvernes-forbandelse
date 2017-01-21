CREATE PROCEDURE [dbo].[SetCharacterStatus]
	@characterId int,
	@isAlive bit
AS
	update Characters set IsAlive = @isAlive where Id = @characterId
RETURN 0
