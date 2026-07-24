using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Contact
{
    public class GetContactsByTenderIdQueryHandler
    {
        private readonly IContactRepository _repository;
        public GetContactsByTenderIdQueryHandler(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ContactModel?>> Handle(GetContactsByTenderIdQuery query, CancellationToken cancellationToken) 
        {
            return await _repository.GetContactsByTenderId(query.Id, cancellationToken);
        }
    }
}
