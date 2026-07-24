namespace iTender.Application.Queries.Credentials
{
    public class GetCredentialByUsernameQuery
    {
        public string Username { get; }
        public GetCredentialByUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
