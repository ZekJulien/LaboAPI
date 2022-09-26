namespace LaboAPI.DTO
{
    public class TournamentDetailsDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string? Location { get; set; }
        public int MinNumberPlayers { get; set; }
        public int MaxNumberPlayers { get; set; }
        public int MinELO { get; set; }
        public int MaxELO { get; set; }
        public int[] CategoryID { get; set; } = new int[0];
        public bool WomenOnly { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public MembersRegisteredDTO[]? MembersRegistered { get; set; }
    }
}
