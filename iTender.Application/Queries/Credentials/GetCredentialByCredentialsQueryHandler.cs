using iTender.Application.Interfaces;
using iTender.Domain.Models;

namespace iTender.Application.Queries.Credentials
{
    public class GetCredentialByCredentialsQueryHandler
    {
        private readonly ICredentialRepository _repository;

        public GetCredentialByCredentialsQueryHandler(ICredentialRepository repository)
        {
            _repository = repository;
        }

        public Task<CredentialModel?> Handle(
            GetCredentialByCredentialsQuery query,
            CancellationToken ct)
        {
            return _repository.GetByCredentialsAsync(
                query.Username,
                query.Password,
                ct);
        }
    }
}
