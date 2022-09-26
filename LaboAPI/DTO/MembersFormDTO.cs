using LaboAPI.Validators;
using System.ComponentModel.DataAnnotations;
using static LaboAPI.Enums.GenderEnums;

namespace LaboAPI.DTO
{
    public class MembersFormDTO
    {
        [Required]
        public string Pseudo { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        public int GenderID { get; set; }
    }
}
