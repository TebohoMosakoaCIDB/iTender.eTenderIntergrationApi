using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Models;
using System.Text.RegularExpressions;

namespace iTender.Application.Utilities
{
    public class ContractServiceUtil
    {
        private readonly ITenderRepository _tenderRepo;

        public ContractServiceUtil(ITenderRepository tenderRepo)
        {
            _tenderRepo = tenderRepo;
        }

        public async Task<RegisterContractModel> GetRegisterContractModel(Guid? tenderId)
        {
            RegisterContractModel model = new RegisterContractModel();
            model.Contract = await GetContractModel(tenderId);
            model.Contractors = new List<ContractorModel>();
            return model;
        }

        public async Task<ContractModel> GetContractModel(Guid? tenderid)
        {
            ContractModel contract = new ContractModel();
            TenderModel tender = new TenderModel();
            if(tenderid != null && tenderid != Guid.Empty)
            {
                tender = await _tenderRepo.GetByIdAsync(tenderid.Value);

                contract.TenderId = tenderid.Value;
                contract.ContractNumber = tender.EmployerTenderNumber;
                contract.ContractTitle = tender.Title;
                contract.TypeOfContractId = tender.TypeOfContractId.Value;
                contract.ClassOfConstructionWorkId = tender.ClassOfConstructionWorksId.Value;
                contract.TenderValueRangeId = tender.TenderValueRangeId.Value;
                contract.EmergingEnterpriseSupportId = tender.EmergingEnterpriseSupport.Value;
                //contract.ExpandedPublicWorksProgrammeId = tender.ExpandedPublicWorksProgramId;
                contract.ContractDescription = tender.TendersInvitedFor;
                //contract.RequiredGrade = string.IsNullOrWhiteSpace(tender.Gra)
                //        ? null
                //        : Regex.Replace(
                //            tender.grading.Trim(),
                //            @"^(\d+)([A-Za-z]+)$",
                //            "$1-$2"
                //          );
                //location details
                contract.ProvinceId = tender.ProvinceId.Value;
                contract.MetroDistrictMunicipalityId = tender.MetroDistrictId.Value;
                contract.LocalMunicipalityId = tender.LocalMunicipalityId.Value;
            }

            return contract;
        }
    }
}
