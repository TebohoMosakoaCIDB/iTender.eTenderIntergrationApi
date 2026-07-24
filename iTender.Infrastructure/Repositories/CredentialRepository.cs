using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace iTender.Infrastructure.Repositories
{
    public class CredentialRepository : ICredentialRepository
    {
        private readonly IOrganizationService _service;
        public CredentialRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        public async Task<CredentialModel?> GetByCredentialsAsync(
            string username,
            string password,
            CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Credentials)
            {
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("nv_name", ConditionOperator.Equal, username),
                        new ConditionExpression("nv_password", ConditionOperator.Equal, password)
                    }
                }
            };

            var entity = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities.FirstOrDefault(), ct);

            return entity == null
                ? null
                : CredentialMapper.ToDomain(entity);
        }

        public async Task<CredentialModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {

            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.Credentials,
                    id,
                    new ColumnSet(true)), ct);

            return entity == null
                ? null
                : CredentialMapper.ToDomain(entity);
        }

        public async Task<CredentialModel?> GetByUsernameAsync(string username, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Credentials)
            {
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression("nv_name", ConditionOperator.Equal, username)
                    }
                }
            };

            var entity = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities.FirstOrDefault(), ct);

            return entity == null
                ? null
                : CredentialMapper.ToDomain(entity);
        }

        public async Task UpdateAsync(CredentialModel model, CancellationToken ct = default)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.Id == Guid.Empty)
                throw new ArgumentException("Id is required for update.");

            var entity = CredentialMapper.ToEntity(model);

            entity.Id = model.Id;

            await Task.Run(() =>
            {
                _service.Update(entity);
            }, ct);
        }
    }
}
