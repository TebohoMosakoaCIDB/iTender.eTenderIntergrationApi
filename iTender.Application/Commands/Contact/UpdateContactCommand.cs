namespace iTender.Application.Commands.Contact
{
    public class UpdateContactCommand
    {
        public Guid Id { get; set; }

        public string? Email { get; set; }

        public string? Telephone { get; set; }

        public string? MobilePhone { get; set; }
    }
}
