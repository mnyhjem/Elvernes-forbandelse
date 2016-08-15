CREATE TABLE [dbo].[Characters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [Name] NVARCHAR(20) NOT NULL, 
    CONSTRAINT [FK_Characters_AspNetUsers] FOREIGN KEY (UserId) REFERENCES [AspNetUsers]([Id])
)
