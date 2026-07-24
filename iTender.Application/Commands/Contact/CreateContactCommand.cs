namespace iTender.Application.Commands.Contact
{
    public class CreateContactCommand
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? Email { get; set; }
        public string? Telephone { get; set; }
        public string? MobilePhone { get; set; }
        public Guid TenderId { get; set; }
        public int ContactType { get; set; }
    }
}
