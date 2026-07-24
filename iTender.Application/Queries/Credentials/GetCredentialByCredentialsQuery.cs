namespace iTender.Application.Queries.Credentials
{
    public class GetCredentialByCredentialsQuery
    {
        public string Username { get; }
        public string Password { get; }

        public GetCredentialByCredentialsQuery(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
