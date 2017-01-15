CREATE PROCEDURE [dbo].[GetCharactersForUser]
	@userId nvarchar(50) = ''
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
c.IsAlive
from 
Characters c 
left outer join Characterlocations loc on c.Id = loc.characterId 
left outer join Worldsections ws on ws.Id = loc.WorldsectionId
where 
c.userid = @userId

