CREATE PROCEDURE [dbo].[GetNpc]
	@id int
AS

select 
npc.Id,
npc.Mode,
npc.Name,
npc.Race,
npc.Status,
npc.Type,
loc.DefaultWorldsectionId, 
loc.DefaultX, 
loc.DefaultY,
loc.CurrentWorldsectionId, 
loc.CurrentX, 
loc.CurrentY,
npc.Level,
npc.Basehealth,
npc.Appearance
from 
Npcs npc 
left outer join NpcLocations loc on npc.Id = loc.NpcId
where npc.id = @id
