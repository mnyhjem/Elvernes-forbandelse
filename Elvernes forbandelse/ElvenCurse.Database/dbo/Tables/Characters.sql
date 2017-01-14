CREATE TABLE [dbo].[Characters]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [Name] NVARCHAR(20) NOT NULL, 
    [IsOnline] BIT NOT NULL DEFAULT 0, 
    [Experience] INT NOT NULL DEFAULT 0, 
    CONSTRAINT [FK_Characters_AspNetUsers] FOREIGN KEY (UserId) REFERENCES [AspNetUsers]([Id])
)
