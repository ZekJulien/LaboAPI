CREATE TABLE [dbo].[TournamentCategory]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[CategoryId] INT NOT NULL, 
    CONSTRAINT [FK_TournamentCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Category]([Id]),
    CONSTRAINT [FK_TournamentCategory_Tournament] FOREIGN KEY ([Id]) REFERENCES [Tournament]([Id]),
)
