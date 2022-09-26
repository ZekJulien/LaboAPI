CREATE TABLE [dbo].[TournamentRegistration]
(
	[TournamentID] UNIQUEIDENTIFIER NOT NULL , 
    [MembersID] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_TournamentRegistration_Tournament] FOREIGN KEY ([TournamentID]) REFERENCES [Tournament]([Id]), 
    CONSTRAINT [FK_TournamentRegistration_Members] FOREIGN KEY ([MembersID]) REFERENCES [Members]([Id])
)
