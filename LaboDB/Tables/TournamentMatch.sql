CREATE TABLE [dbo].[TournamentMatch]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID() , 
    [TournamentID] UNIQUEIDENTIFIER NOT NULL, 
    [WhiteID] UNIQUEIDENTIFIER NOT NULL, 
    [BlackID] UNIQUEIDENTIFIER NOT NULL, 
    [RoundNumber] SMALLINT NOT NULL, 
    [ResultID] TINYINT NOT NULL, 
    CONSTRAINT [PK_TournamentMatch] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_TournamentMatch_TournamentResult] FOREIGN KEY ([ResultID]) REFERENCES [TournamentResult]([Id]), 
    CONSTRAINT [FK_TournamentMatch_Black_Members] FOREIGN KEY ([BlackID]) REFERENCES [Members]([Id]),
    CONSTRAINT [FK_TournamentMatch_White_Members] FOREIGN KEY ([WhiteID]) REFERENCES [Members]([Id]), 
    CONSTRAINT [FK_TournamentMatch_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [Tournament]([Id]),
)
