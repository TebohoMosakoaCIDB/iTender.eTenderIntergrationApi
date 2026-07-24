using iTender.Application.Interfaces;

namespace iTender.Application.Commands.Contact
{
    public class DeleteContactCommandHandler
    {
        private readonly IContactRepository _repository;

        public DeleteContactCommandHandler(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(
            DeleteContactCommand command,
            CancellationToken ct)
        {
            await _repository.DeleteAsync(command.Id, ct);
        }
    }
}
