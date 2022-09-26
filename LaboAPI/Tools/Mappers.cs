using LaboAPI.DTO;
using DAL.Entities;

namespace LaboAPI.Tools
{
    public static class Mappers
    {
        public static MembersFormDTO ToAPI(this MembersEntities member)
        {
            return new MembersFormDTO
            {
                Pseudo = member.Pseudo,
                Email = member.Email,
                Password = member.Password,
                BirthDate = member.BirthDate,
                GenderID = member.GenderID,
            };
        }

        public static MembersEntities ToDAL(this MembersFormDTO member)
        {
            return new MembersEntities
            {
                Pseudo = member.Pseudo,
                Email = member.Email,
                Password = member.Password,
                BirthDate = member.BirthDate,
                GenderID = member.GenderID,
            };
        }

        public static MembersRegisteredDTO RegisteredToAPI(this MembersEntities member)
        {
            return new MembersRegisteredDTO
            {
                Id = member.Id,
                Pseudo = member.Pseudo,
                Email = member.Email,
                BirthDate = member.BirthDate,
                GenderID = member.GenderID,
                RoleID = member.RoleID,
                ELO = member.ELO,
            };
        }



        public static TournamentFormDTO ToAPI(this TournamentEntities tournament)
        {
            return new TournamentFormDTO
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Location = tournament.Location,
                MinNumberPlayers = tournament.MinNumberPlayers,
                MaxNumberPlayers = tournament.MaxNumberPlayers,
                MinELO = tournament.MinELO,
                MaxELO = tournament.MaxELO,
                CategoryID = tournament.CategoryID,
                WomenOnly = tournament.WomenOnly,
                RegistrationEndDate = tournament.RegistrationEndDate,
            };
        }

        public static TournamentEntities ToDAL(this TournamentFormDTO tournament)
        {
            return new TournamentEntities
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Location = tournament.Location,
                MinNumberPlayers = tournament.MinNumberPlayers,
                MaxNumberPlayers = tournament.MaxNumberPlayers,
                MinELO = tournament.MinELO,
                MaxELO = tournament.MaxELO,
                CategoryID = tournament.CategoryID,
                WomenOnly = tournament.WomenOnly,
                RegistrationEndDate = tournament.RegistrationEndDate,
            };
        }
    }
}
