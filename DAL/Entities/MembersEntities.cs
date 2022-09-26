using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class MembersEntities
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Pseudo { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; }
        public int GenderID { get; set; }
        public int ELO { get; set; }
        public int RoleID { get; set; }
    }
}
