CREATE TABLE [dbo].[Npcs]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Type] INT NOT NULL, 
    [Name] NVARCHAR(20) NOT NULL, 
    [Status] INT NOT NULL, 
    [Mode] INT NOT NULL DEFAULT 0, 
    [Race] INT NOT NULL DEFAULT 0, 
    [Level] INT NOT NULL DEFAULT 1, 
    [Basehealth] INT NOT NULL DEFAULT 70, 
    [Appearance] NVARCHAR(500) NOT NULL DEFAULT '{"Sex":1,"Body":4,"Eyecolor":0,"Nose":0,"Ears":2,"Facial":{"Type":0,"Color":0},"Hair":{"Type":20,"Color":7}}', 
    [Equipment] NVARCHAR(4000) NULL
)
