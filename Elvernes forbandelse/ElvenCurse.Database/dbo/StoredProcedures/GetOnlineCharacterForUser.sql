CREATE PROCEDURE [dbo].[GetOnlineCharacterForUser]
	@userId nvarchar(50)
AS
select 
c.Id, 
c.Name, 
WorldsectionId, 
X, 
Y
from 
Characters c 
left outer join Characterlocations loc on c.Id = loc.characterId 
left outer join Worldsections ws on ws.Id = loc.WorldsectionId
where 
c.userid = @userId and 
c.IsOnline = 1

