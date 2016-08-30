CREATE TABLE [dbo].[InteractiveObjects]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(200) NOT NULL, 
    [Type] INT NOT NULL, 
    [WorldsectionId] INT NOT NULL, 
    [X] INT NOT NULL, 
    [Y] INT NOT NULL
)
