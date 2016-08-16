﻿--exec GetCharactersForUser '2f40b9b8-3abe-4979-8ad6-7c954c2bddd6'

select 
c.Id, 
c.Name, 
WorldsectionId, 
X, 
Y,
ws.Jsonname
from 
Characters c 
left outer join Characterlocations loc on c.Id = loc.characterId 
left outer join Worldsections ws on ws.Id = loc.WorldsectionId
where 
c.id = 3

--select * from Characters