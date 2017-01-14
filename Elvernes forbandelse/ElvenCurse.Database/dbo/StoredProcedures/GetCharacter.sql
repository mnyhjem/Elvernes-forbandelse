CREATE PROCEDURE [dbo].[GetCharacter]
	@characterId int,
	@userId nvarchar(50)
AS
select 
c.Id, 
c.Name, 
WorldsectionId, 
X, 
Y,
ws.Name as worldsectionname,
c.Experience as AccumulatedExperience
from 
Characters c 
left outer join Characterlocations loc on c.Id = loc.characterId 
left outer join Worldsections ws on ws.Id = loc.WorldsectionId
where 
c.userid = @userId and 
c.id = @characterId;

