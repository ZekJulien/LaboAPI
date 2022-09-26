using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class TournamentDetailsEntities
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Location { get; set; } = String.Empty;
        public int MinNumberPlayers { get; set; }
        public int MaxNumberPlayers { get; set; }
        public int MinELO { get; set; }
        public int MaxELO { get; set; }
        public IEnumerable<int> CategoryID { get; set; } = Enumerable.Empty<int>();
        public int StatusID { get; set; }
        public int Round { get; set; }
        public bool WomenOnly { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public DateTime TournamentCreateDate { get; set; }
        public DateTime TournamentUpdateDate { get; set; }
        public IEnumerable<MembersRegisteredEntities>? MembersRegistered { get; set; }
    }
}
