using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ITournamentServices
    {
        IEnumerable<TournamentIndexEntities> GetAll(Guid IdMember = default);
        void Add(TournamentEntities tournament);
        void Delete(Guid Id);
        DateTime GetRegistrationEndDate(Guid Id);
        byte GetStatus(Guid Id);
        TournamentDetailsEntities GetDetails(Guid Id);
        IEnumerable<MembersRegisteredEntities> GetRegisteredMembers(Guid Id);
        void TournamentInscription(Guid TournamentId, Guid MemberId);
        void TournamentUnscription(Guid TournamentId, Guid MemberId);
        IEnumerable<int> GetCategory(Guid Id);
        bool AddCategory(Guid Id, IEnumerable<int> number);
        bool canRegister(Guid IdTournament = default(Guid), Guid IdMember = default(Guid));
    }
}
