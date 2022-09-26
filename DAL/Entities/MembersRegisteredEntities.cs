using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class MembersRegisteredEntities
    {
        public Guid Id { get; set; }
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int ELO { get; set; }
        public int GenderID { get; set; }
        public int RoleID { get; set; }
    }
}
