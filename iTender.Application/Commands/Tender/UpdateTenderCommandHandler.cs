using iTender.Application.Interfaces;

namespace iTender.Application.Commands.Tender
{
    public class UpdateTenderCommandHandler
    {
        private readonly ITenderRepository _repo;

        public UpdateTenderCommandHandler(ITenderRepository repo)
        {
            _repo = repo;
        }

        public Task<Guid> Handle(UpdateTenderCommand command, CancellationToken ct = default)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));

            return _repo.ChangeTenderStatusAsync(command.Model, ct);
        }
    }
}
