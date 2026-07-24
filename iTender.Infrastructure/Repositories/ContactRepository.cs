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
    public class ContactRepository : IContactRepository
    {
        private readonly IOrganizationService _service;

        public ContactRepository(ICrmServiceFactory factory)
        {
            _service = factory.Create();
        }

        public async Task<ContactModel?> GetByIdAsync(
            Guid id,
            CancellationToken ct = default)
        {
            var entity = await Task.Run(() =>
                _service.Retrieve(
                    CrmEntityNames.Contact,
                    id,
                    new ColumnSet(true)), ct);

            return entity == null
                ? null
                : ContactMapper.ToDomain(entity);
        }

        public async Task<Guid> CreateAsync(
            ContactModel model,
            CancellationToken ct = default)
        {
            var entity = ContactMapper.ToEntity(model);

            return await Task.Run(() =>
                _service.Create(entity), ct);
        }

        public async Task UpdateAsync(
            ContactModel model,
            CancellationToken ct = default)
        {
            var entity = ContactMapper.ToEntity(model);

            await Task.Run(() =>
                _service.Update(entity), ct);
        }

        public async Task DeleteAsync(
            Guid id,
            CancellationToken ct = default)
        {
            await Task.Run(() =>
                _service.Delete(CrmEntityNames.Contact, id), ct);
        }

        public List<ContactModel> GetUserByEmailAddress(string emailAddress, string[] columns)
        {
            ConditionExpression condition = new ConditionExpression
            {
                AttributeName = ContactFields.Email,
                Operator = ConditionOperator.Equal
            };
            condition.Values.Add(emailAddress);

            ConditionExpression activeCondition = new ConditionExpression
            {
                AttributeName = ApplicationFields.StateCode,
                Operator = ConditionOperator.Equal
            };
            activeCondition.Values.Add(StateCodes.StateCode_Active);

            ConditionExpression externalUsersCondition = new ConditionExpression
            {
                AttributeName = ContactFields.ContactType,
                Operator = ConditionOperator.In
            };
            externalUsersCondition.Values.Add(100000000);

            FilterExpression filter = new FilterExpression
            {
                FilterOperator = LogicalOperator.And
            };

            filter.Conditions.AddRange(new[]
            {
                condition,
                externalUsersCondition,
                activeCondition
            });

            QueryExpression query = new QueryExpression
            {
                EntityName = CrmEntityNames.Contact,
                Criteria = filter
            };

            query.ColumnSet = new ColumnSet(true);

            EntityCollection contacts = _service.RetrieveMultiple(query);

            return contacts.Entities
                .Select(ContactMapper.ToDomain)
                .ToList(); 
        }

        public Task<List<PermissionModel>> GetContactsPermissions(Guid contactId, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.ContactPermissions)
            {
                ColumnSet = new ColumnSet(true),
                Distinct = true
            };

            query.Criteria.AddCondition(
                ContactPermissionFields.IndividualId,
                ConditionOperator.Equal,
                contactId);

            var response = _service.RetrieveMultiple(query);

            return Task.FromResult(
                response.Entities
                    .Select(PermissionMapper.ToDomain)
                    .ToList());
        }

        public Task<List<ContactModel>> GetContactsByTenderId(Guid Id, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Contact)
            {
                ColumnSet = new ColumnSet(true),
                Criteria = new FilterExpression(LogicalOperator.And),
                Distinct = true
            };

            query.Criteria.AddCondition(
                ContactFields.TenderID,
                ConditionOperator.Equal,
                Id);

            //query.Criteria.AddCondition(
            //    ContactFields.ContactType,
            //    ConditionOperator.Equal,
            //    10000001);

            var response = _service.RetrieveMultiple(query);

            return Task.FromResult(
                response.Entities
                    .Select(ContactMapper.ToDomain)
                    .ToList());
        }
    }
}
