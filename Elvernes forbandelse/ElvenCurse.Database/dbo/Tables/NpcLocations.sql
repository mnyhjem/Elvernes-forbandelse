CREATE TABLE [dbo].[NpcLocations]
(
	[NpcId] INT NOT NULL PRIMARY KEY, 
    [DefaultWorldsectionId] INT NOT NULL, 
    [DefaultX] INT NOT NULL, 
    [DefaultY] INT NOT NULL, 
    [CurrentWorldsectionId] INT NOT NULL, 
    [CurrentX] INT NOT NULL, 
    [CurrentY] INT NOT NULL, 
    CONSTRAINT [FK_NpcLocations_Npcs] FOREIGN KEY ([NpcId]) REFERENCES [Npcs]([Id]),
)
