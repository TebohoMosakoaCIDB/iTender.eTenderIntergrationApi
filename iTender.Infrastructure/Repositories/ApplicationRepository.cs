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
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly IOrganizationService _service;

        public ApplicationRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        public async Task<ApplicationModel?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.Application,
                    id,
                    new ColumnSet(true)), ct);

            return entity == null ? null : ApplicationMapper.ToDomain(entity);
        }

        public async Task<ApplicationModel?> GetByApplicationNumberAsync(string applicationNumber, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(applicationNumber))
                return null;

            var query = new QueryExpression(CrmEntityNames.Application)
            {
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(
                            ApplicationFields.ApplicationNumber,
                            ConditionOperator.Equal,
                            applicationNumber)
                    }
                }
            };

            var entity = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities.FirstOrDefault(), ct);

            return entity == null ? null : ApplicationMapper.ToDomain(entity);
        }

        public async Task<IEnumerable<ApplicationModel>> SearchAsync(string searchText, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Application)
            {
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    FilterOperator = LogicalOperator.Or,
                    Conditions =
                    {
                        new ConditionExpression(ApplicationFields.ApplicationNumber, ConditionOperator.Like, $"%{searchText}%"),
                        new ConditionExpression(ApplicationFields.Type, ConditionOperator.Like, $"%{searchText}%")
                    }
                }
            };

            var entities = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities, ct);

            return entities.Select(ApplicationMapper.ToDomain);
        }

        public async Task<Guid> CreateAsync(ApplicationModel model, CancellationToken ct = default)
        {
            var entity = ApplicationMapper.ToEntity(model);

            return await Task.Run(() =>
                _service.Create(entity), ct);
        }

        public async Task UpdateAsync(ApplicationModel model, CancellationToken ct = default)
        {
            var entity = ApplicationMapper.ToEntity(model);

            await Task.Run(() =>
                _service.Update(entity), ct);
        }

        public async Task DeleteAsync(Guid id, CancellationToken ct = default)
        {
            await Task.Run(() =>
                _service.Delete(CrmEntityNames.Application, id), ct);
        }
    }
}
