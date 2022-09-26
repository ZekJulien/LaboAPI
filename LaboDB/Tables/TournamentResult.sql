CREATE TABLE [dbo].[TournamentResult]
(
	[Id] TINYINT NOT NULL IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [PK_TournamentResult] PRIMARY KEY ([Id]) 
)
