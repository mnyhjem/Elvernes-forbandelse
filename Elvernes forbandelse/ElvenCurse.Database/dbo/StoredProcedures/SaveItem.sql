CREATE PROCEDURE [dbo].[SaveItem]
	@id int,
	@category int,
	@type int,
	@name nvarchar(50),
	@description nvarchar(250),
	@imagepath nvarchar(100)
AS



begin
	if not exists (select * from Items where Id = @id)
		begin
			insert into Items (Category, Type, Name, Description, Imagepath) 
			values(@category, @type, @name, @description, @imagepath);
			select SCOPE_IDENTITY();
		end
	else
		begin
			update Items set Category=@category, Type=@type, Name=@name, Description=@description, Imagepath=@imagepath where Id = @id
		end
end