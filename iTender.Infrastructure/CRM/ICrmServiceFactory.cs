using Microsoft.Xrm.Sdk;

namespace iTender.Infrastructure.CRM
{
    public interface ICrmServiceFactory
    {
        IOrganizationService Create();
    }
}
