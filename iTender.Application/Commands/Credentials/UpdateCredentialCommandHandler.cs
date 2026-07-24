using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Commands.Credentials
{
    public class UpdateCredentialCommandHandler
    {
        private readonly ICredentialRepository _repository;

        public UpdateCredentialCommandHandler(ICredentialRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateCredentialCommand command, CancellationToken ct)
        {
            var model = new CredentialModel
            {
                Id = command.Id,
                LastLogin = DateTime.Now,
                Username = command.Username,
                Password = command.Password,
                IncorrectLoginCount = command.IncorrectLoginCount
            };

            await _repository.UpdateAsync(model, ct);
        }
    }
}
