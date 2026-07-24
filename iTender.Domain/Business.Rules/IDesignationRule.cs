using iTender.Domain.Models;

namespace iTender.Domain.Business.Rules
{
    public interface IDesignationRule
    {
        DesignationGrade GetGrade(List<ContractorModel> contractors);
    }
}
