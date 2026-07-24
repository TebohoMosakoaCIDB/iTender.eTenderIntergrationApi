namespace iTender.Domain.Models
{
    public class CredentialModel
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsLocked { get; set; }
        public bool MfaEnabled { get; set; }
        public int? IncorrectLoginCount { get; set; }
        //public int? PreferredMfaMethod { get; set; }
        public bool ForcePasswordChange { get; set; }
        //public int? StatusCode { get; set; }
    }
}
