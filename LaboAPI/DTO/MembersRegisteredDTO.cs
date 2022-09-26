using System.ComponentModel.DataAnnotations;

namespace LaboAPI.DTO
{
    public class MembersRegisteredDTO
    {
        public Guid Id { get; set; }
        public string Pseudo { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        public int ELO { get; set; }
        public int GenderID { get; set; }
        public int RoleID { get; set; }
    }
}
