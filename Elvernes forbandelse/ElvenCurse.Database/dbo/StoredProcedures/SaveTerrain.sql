CREATE PROCEDURE [dbo].[SaveTerrain]
	@id int,
	@filename nvarchar(50),
	@data text
AS


begin
	if not exists (select * from Terrains where Id = @id)
		begin
			insert into Terrains (Filename, Data) 
			values(@filename, @data);
			select SCOPE_IDENTITY();
		end
	else
		begin
			update Terrains set Data=@data, Filename=@filename where Id = @id
		end
end