CREATE PROCEDURE [dbo].[QueueMessagequeueElement]
	@messagetype int,
	@parameters nvarchar(500)
AS
	insert into MessageQueue (Messagetype,Parameters) values(@messagetype, @parameters)

