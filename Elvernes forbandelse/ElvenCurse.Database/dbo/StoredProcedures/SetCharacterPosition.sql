CREATE PROCEDURE [dbo].[SetCharacterPosition]
	@characterId int,
	@worldsectionId int,
	@x int,
	@y int
AS

begin
	if not exists (select * from Characterlocations where characterId = @characterId)
		begin
			insert into Characterlocations (characterId, WorldsectionId, X, Y) 
			values(@characterId, @worldsectionId, @x, @y);
			select SCOPE_IDENTITY();
		end
	else
		begin
			update Characterlocations set WorldsectionId = @worldsectionId, X = @x, Y = @y where characterId = @characterId
		end
end