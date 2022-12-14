CREATE TABLE [dbo].[Tournament]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Name] NVARCHAR(50) NOT NULL, 
    [Location] NVARCHAR(50) NULL, 
    [MinNumberPlayers] SMALLINT NOT NULL, 
    [MaxNumberPlayers] SMALLINT NOT NULL, 
    [MinELO] SMALLINT NOT NULL, 
    [MaxELO] SMALLINT NOT NULL, 
    [StatusID] TINYINT NOT NULL DEFAULT 1, 
    [Round] SMALLINT NOT NULL DEFAULT 0, 
    [WomenOnly] BIT NOT NULL, 
    [RegistrationEndDate] DATETIME NOT NULL, 
    [TournamentCreateDate] DATETIME NOT NULL DEFAULT GETDATE(), 
    [TournamentUpdateDate] DATETIME NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [PK_Tournament] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tournament_Status] FOREIGN KEY ([StatusID]) REFERENCES [Status]([Id]), 
    CONSTRAINT [CK_Tournament_MinELO] CHECK (MinELO BETWEEN 0 AND 3000), 
    CONSTRAINT [CK_Tournament_MaxELO] CHECK (MaxELO BETWEEN 0 AND 3000), 
    CONSTRAINT [CK_Tournament_MinNumberPlayers] CHECK (MinNumberPlayers BETWEEN 2 AND 32), 
    CONSTRAINT [CK_Tournament_MaxNumberPlayers] CHECK (MaxNumberPlayers BETWEEN 2 AND 32), 
)