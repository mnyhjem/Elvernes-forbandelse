CREATE TABLE [dbo].[MessageQueue]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Messagetype] INT NOT NULL, 
    [Parameters] NVARCHAR(500) NOT NULL, 
    [Queuetime] DATETIME NOT NULL DEFAULT GETDATE(), 
    [Processed] BIT NOT NULL DEFAULT 0
)
