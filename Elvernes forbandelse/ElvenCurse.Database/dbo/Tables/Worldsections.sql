CREATE TABLE [dbo].[Worldsections]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Json] TEXT NOT NULL, 
    [Mapchange_Right] INT NOT NULL, 
    [Mapchange_Left] INT NOT NULL, 
    [Mapchange_Up] INT NOT NULL, 
    [Mapchange_Down] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL
)
