CREATE PROCEDURE [dbo].[GetMessageQueue]
	
AS

select q.Id,q.Messagetype,q.Parameters,q.Queuetime from messagequeue q where q.Processed = 0 order by queuetime