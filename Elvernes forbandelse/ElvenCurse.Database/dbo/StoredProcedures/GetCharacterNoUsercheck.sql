CREATE PROCEDURE [dbo].[GetCharacterNoUsercheck]
	@characterId int
AS
select 
c.Id, 
c.Name, 
WorldsectionId, 
X, 
Y,
ws.Name as worldsectionname,
c.Experience as AccumulatedExperience,
c.BaseHealth,
c.IsAlive,
c.Appearance
from 
Characters c 
left outer join Characterlocations loc on c.Id = loc.characterId 
left outer join Worldsections ws on ws.Id = loc.WorldsectionId
where 
c.id = @characterId;

