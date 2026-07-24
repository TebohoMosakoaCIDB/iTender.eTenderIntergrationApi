using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contact
{
    public class GetContactByIdQueryHandler
    {
        private readonly IContactRepository _repository;

        public GetContactByIdQueryHandler(
            IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<ContactModel?> Handle(
            GetContactByIdQuery query,
            CancellationToken ct)
        {
            return await _repository.GetByIdAsync(query.Id, ct);
        }
    }
}
