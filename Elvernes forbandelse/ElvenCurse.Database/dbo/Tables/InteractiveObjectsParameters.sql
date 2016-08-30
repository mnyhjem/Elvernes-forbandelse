CREATE TABLE [dbo].[InteractiveObjectsParameters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Interactiveobject_Id] INT NOT NULL, 
    [Key] NVARCHAR(50) NOT NULL, 
    [Value] NVARCHAR(500) NOT NULL, 
    CONSTRAINT [FK_InteractiveObjectsParameters_Interactiveobjects] FOREIGN KEY ([Interactiveobject_Id]) REFERENCES [InteractiveObjects]([Id])
)
