using iTender.Application.Interfaces;

namespace iTender.Application.Commands.Tender
{
    public class DeleteTenderCommandHandler
    {
        private readonly ITenderRepository _repository;

        public DeleteTenderCommandHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public Task Handle(DeleteTenderCommand command, CancellationToken ct = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return _repository.DeleteAsync(command.Id, ct);
        }
    }
}
