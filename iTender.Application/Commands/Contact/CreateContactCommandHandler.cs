using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Commands.Contact
{
    public class CreateContactCommandHandler
    {
        private readonly IContactRepository _repository;

        public CreateContactCommandHandler(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(
            CreateContactCommand command,
            CancellationToken ct)
        {
            var model = new ContactModel
            {
                Id = Guid.Empty,

                FirstName = command.FirstName,
                LastName = command.LastName,

                Email = command.Email,
                Telephone = command.Telephone,
                MobilePhone = command.MobilePhone,
                TenderId = command.TenderId,
                ContactType = command.ContactType
            };

            return await _repository.CreateAsync(model, ct);
        }
    }
}