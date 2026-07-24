using iTender.Domain.Models;

namespace iTender.Application.DTOs
{
    public class RegisterContractModel
    {
        private ContractModel _contractModel;
        private List<ContractorModel> _contractors;

        public ContractModel Contract { get => _contractModel; set => _contractModel = value; }
        public List<ContractorModel> Contractors { get => _contractors; set => _contractors = value; }

        public RegisterContractModel()
        {
            _contractModel = new ContractModel();
            _contractors = new List<ContractorModel>();
        }
    }
}
