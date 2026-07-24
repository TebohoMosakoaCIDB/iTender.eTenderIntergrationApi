namespace iTender.Application.Commands.Credentials
{
    public class UpdateCredentialCommand
    {
        public Guid Id { get; set; }
        public int? IncorrectLoginCount { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }

        public UpdateCredentialCommand(Guid id, int incorrectLoginCount, string username, string password)
        {
            Id = id;
            IncorrectLoginCount = incorrectLoginCount;
            Username = username;
            Password = password;
        }
    }
}
