namespace LaboAPI.DTO
{
    public class TokenDTO
    {
        public TokenDTO(string token, MembersRegisteredDTO membersRegistered)
        {
            Token = token;
            MembersRegistered = membersRegistered;
        }
 
        public string Token { get; set; } = string.Empty;
        public MembersRegisteredDTO MembersRegistered { get; set; }

    }
}
