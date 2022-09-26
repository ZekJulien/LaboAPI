using System.ComponentModel.DataAnnotations;

namespace LaboAPI.DTO
{
    public class TournamentIndexDTO
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        public string Location { get; set; } = String.Empty;
        [Required]
        public int MinNumberPlayers { get; set; }
        [Required]
        public int MaxNumberPlayers { get; set; }
        [Required]
        public int MinELO { get; set; }
        [Required]
        public int MaxELO { get; set; }
        [Required]
        public IEnumerable<int> CategoryID { get; set; } = Enumerable.Empty<int>();
        [Required]
        public bool WomenOnly { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public int Players { get; set; }
        public bool canRegister { get; set; }
        public bool IsRegistered { get; set; }
    }
}
