CREATE PROCEDURE [dbo].[SaveWorldsection]
	@Id int,
	@Mapchange_Down int,
	@Mapchange_Up int,
	@Mapchange_Left int,
	@Mapchange_Right int,
	@Json text = NULL,
	@Name nvarchar(50)
AS
	

begin
	if not exists (select * from Worldsections where Id = @Id)
		begin
			insert into Worldsections ([Json], Mapchange_Down, Mapchange_Left, Mapchange_Right, Mapchange_Up, Name) 
			values(@Json, @Mapchange_Down, @Mapchange_Left, @Mapchange_Right, @Mapchange_Up, @Name);
			select SCOPE_IDENTITY();
		end
	else
		begin
			if @Json is null
				update Worldsections set Mapchange_Down=@Mapchange_Down, Mapchange_Left=@Mapchange_Left, Mapchange_Right=@Mapchange_Right, Mapchange_Up=@Mapchange_Up, Name=@Name where Id = @Id
			else
				update Worldsections set [Json]=@json, Mapchange_Down=@Mapchange_Down, Mapchange_Left=@Mapchange_Left, Mapchange_Right=@Mapchange_Right, Mapchange_Up=@Mapchange_Up, Name=@Name where Id = @Id
		end
end