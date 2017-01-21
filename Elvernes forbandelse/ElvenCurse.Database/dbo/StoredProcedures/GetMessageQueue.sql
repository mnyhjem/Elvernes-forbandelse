CREATE PROCEDURE [dbo].[GetMessageQueue]
	
AS

select q.Id,q.Messagetype,q.Parameters,q.Queuetime from MessageQueue q where q.Processed = 0 order by Queuetime