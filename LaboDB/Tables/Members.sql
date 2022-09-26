CREATE TABLE [dbo].[Members]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Pseudo] NVARCHAR(50) NOT NULL, 
    [Email] NVARCHAR(255) NOT NULL, 
    [Password] NVARCHAR(255) NOT NULL, 
    [BirthDate] DATE NOT NULL, 
    [GenderID] TINYINT NOT NULL, 
    [ELO] SMALLINT NOT NULL DEFAULT 1200, 
    [RoleID] TINYINT NOT NULL DEFAULT 2, 
    CONSTRAINT [PK_Members] PRIMARY KEY ([Id]),
    CONSTRAINT [UK_Members_Pseudo] UNIQUE ([Pseudo]),
    CONSTRAINT [UK_Members_Email] UNIQUE ([Email]), 
    CONSTRAINT [FK_Members_Genders] FOREIGN KEY ([GenderID]) REFERENCES [Genders]([Id]), 
    CONSTRAINT [FK_Members_Roles] FOREIGN KEY ([RoleID]) REFERENCES [Roles]([Id]), 
    CONSTRAINT [CK_Members_ELO] CHECK (ELO BETWEEN 0 AND 3000), 
)
