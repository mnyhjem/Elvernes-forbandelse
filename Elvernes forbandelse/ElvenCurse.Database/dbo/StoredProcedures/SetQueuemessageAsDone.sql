CREATE PROCEDURE [dbo].[SetQueuemessageAsDone]
	@id int,
	@errorMessage nvarchar(4000)
AS
	update MessageQueue set Processed = 1, ErrorMessage = @errorMessage where Id = @id
RETURN 0
