CREATE PROCEDURE [dbo].[AddNewCharacter]
	@userId nvarchar(50),
	@name nvarchar(20)
AS
	
begin
	if not exists (select * from Characters where Name = @name)
		begin
			insert into Characters (UserId, Name) values(@userId, @name);
			select SCOPE_IDENTITY();
		end
end