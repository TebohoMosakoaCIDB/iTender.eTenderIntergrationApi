using iTender.Application.Interfaces;

namespace iTender.Application.Commands.Tender
{
    public class CreateTenderCommandHandler
    {
        private readonly ITenderRepository _repository;

        public CreateTenderCommandHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public Task<Guid> Handle(CreateTenderCommand command, CancellationToken ct = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return _repository.CreateAsync(command.Model, ct);
        }
    }
}
