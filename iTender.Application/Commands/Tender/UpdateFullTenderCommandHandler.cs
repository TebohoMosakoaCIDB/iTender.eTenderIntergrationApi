using iTender.Application.Interfaces;

namespace iTender.Application.Commands.Tender
{
    public class UpdateFullTenderCommandHandler
    {
        private readonly ITenderRepository _repository;

        public UpdateFullTenderCommandHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(UpdateFullTenderCommand command, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            if (command.Model == null)
                throw new ArgumentNullException(nameof(command.Model));

            if (command.Model.Id == Guid.Empty)
                throw new ArgumentException("Tender Id is required.");

            // optional: existence check
            var existing = await _repository.GetByIdAsync(command.Model.Id, ct);

            if (existing == null)
                throw new KeyNotFoundException(
                    $"Tender '{command.Model.Id}' not found.");

            return await _repository.UpdateAsync(command.Model, ct);
        }
    }
}
