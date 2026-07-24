using iTender.Application.Interfaces;

namespace iTender.Application.Commands.Contact
{
    public class UpdateContactCommandHandler
    {
        private readonly IContactRepository _repository;

        public UpdateContactCommandHandler(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(
            UpdateContactCommand command,
            CancellationToken ct)
        {
            var existing =
                await _repository.GetByIdAsync(command.Id, ct);

            if (existing == null)
                throw new Exception("Contact not found.");

            existing.Email = command.Email;
            existing.Telephone = command.Telephone;
            existing.MobilePhone = command.MobilePhone;

            await _repository.UpdateAsync(existing, ct);
        }
    }
}
