using iTender.Application.Interfaces;
using iTender.Domain.Constants;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IOrganizationService _service;

        public AccountRepository(ICrmServiceFactory crmFactory)
        {
            _service = crmFactory.Create();
        }
        public Task<Guid> CreateAsync(AccountModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            await DeleteAsync(CrmEntityNames.Account, id, ct);
        }

        private Task DeleteAsync(string entityName, Guid id, CancellationToken ct)
        {
            return Task.Run(() =>
            {
                _service.Delete(entityName, id);
            }, ct);
        }

        public async Task<AccountModel?> GetByCRSNumberAsync(string crsNumber, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(crsNumber))
                throw new ArgumentException("CRS number cannot be null or empty.", nameof(crsNumber));

            var query = new QueryExpression(CrmEntityNames.Account)
            {
                ColumnSet = new ColumnSet(true),
                Criteria =
        {
            Conditions =
            {
                new ConditionExpression(
                    AccountFields.CrsNumber,
                    ConditionOperator.Equal,
                    crsNumber.Trim())
            }
        }
            };

            var entity = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities.FirstOrDefault(), ct);

            if (entity == null)
                return null;

            return AccountMapper.ToDomain(entity);
        }

        public async Task<AccountModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Id cannot be empty.", nameof(id));

            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.Account,
                    id,
                    new ColumnSet(true)
                ), ct);

            if (entity == null)
                return null;

            return AccountMapper.ToDomain(entity);
        }

        public Task<IReadOnlyList<AccountModel>> GetSanctionedAccountsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AccountModel>> SearchAsync(string searchText, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(AccountModel model, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
