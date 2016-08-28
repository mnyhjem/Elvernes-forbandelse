CREATE PROCEDURE [dbo].[GetAllInteractiveObjects]

AS

select 
Id,
Name,
Type,
WorldsectionId, 
X, 
Y
from 
InteractiveObjects

