CREATE TABLE [dbo].[Characters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [Name] NVARCHAR(20) NOT NULL, 
    [IsOnline] BIT NOT NULL DEFAULT 0, 
    [Experience] INT NOT NULL DEFAULT 0, 
    [BaseHealth] INT NOT NULL DEFAULT 125, 
    [IsAlive] BIT NOT NULL DEFAULT 1, 
    [Appearance] NVARCHAR(4000) NOT NULL DEFAULT '{"Sex":1,"Body":4,"Eyecolor":0,"Nose":0,"Ears":2,"Facial":{"Type":0,"Color":0},"Hair":{"Type":20,"Color":7}}', 
    [Type] INT NOT NULL DEFAULT 0, 
    [Equipment] NVARCHAR(4000) NULL, 
    CONSTRAINT [FK_Characters_AspNetUsers] FOREIGN KEY (UserId) REFERENCES [AspNetUsers]([Id])
)
