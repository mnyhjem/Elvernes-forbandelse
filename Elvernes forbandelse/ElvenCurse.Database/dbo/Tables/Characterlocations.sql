﻿CREATE TABLE [dbo].[Characterlocations]
(
	[characterId] INT NOT NULL PRIMARY KEY, 
    [WorldsectionId] INT NOT NULL, 
    [X] INT NOT NULL, 
    [Y] INT NOT NULL, 
    CONSTRAINT [FK_Characterlocations_Worldsections] FOREIGN KEY ([WorldsectionId]) REFERENCES [Worldsections]([Id])
)
