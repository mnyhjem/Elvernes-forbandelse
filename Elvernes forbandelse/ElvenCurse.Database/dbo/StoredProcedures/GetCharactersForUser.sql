CREATE PROCEDURE [dbo].[GetCharactersForUser]
	@userId nvarchar(50) = ''
AS
	select Id,Name from characters where userid = @userId;

