using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Credentials
{
    public class GetCredentialByUsernameQueryHandler
    {
        private readonly ICredentialRepository _repository;

        public GetCredentialByUsernameQueryHandler(ICredentialRepository repository)
        {
            _repository = repository;
        }

        public Task<CredentialModel?> Handle(
            GetCredentialByUsernameQuery query,
            CancellationToken ct)
        {
            return _repository.GetByUsernameAsync(query.Username, ct);
        }
    }
}
