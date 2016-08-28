CREATE PROCEDURE [dbo].[GetAllInteractiveObjectParameters]
	@ioid int
AS

select 
Id,
[Key],
[Value] 

from InteractiveObjectsParameters where Id = @ioid
