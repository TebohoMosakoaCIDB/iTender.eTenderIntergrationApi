using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Application.Utilities;
using iTender.Domain.Business.Rules;
using iTender.Domain.Constants;
using iTender.Domain.Enums;
using iTender.Domain.Models;
using iTender.Infrastructure.CRM;
using iTender.Infrastructure.Mappers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Repositories
{
    public class JointVentureRepository : IJointVentureRepository
    {
        private readonly IOrganizationService _service;

        public JointVentureRepository(ICrmServiceFactory crmFactory)
        {
            _service = crmFactory.Create();
        }

        public async Task<JVGradingDesignationModel> GetRecommendedGrade(JVGradingDesignationModel model, CancellationToken ct = default)
        {
            string recommendedGrade = string.Empty;

            JVGradingDesignationModel newModel = new JVGradingDesignationModel();
            newModel.Success = false;
            newModel.DateCalculated = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
            newModel.Designation = model.Designation;
            newModel.ClassOfWork = model.ClassOfWork;

            newModel.Contractors = GetJVContractors(model, ct);

            var lead = newModel.Contractors.FirstOrDefault();

            if (lead?.StatusText != "Active" +
                "")
            {
                newModel.RecommendedGrade = string.Format("Lead Contractor does not have an Active registration status.");
            }

            else if (lead != null && lead.Id != Guid.Empty)
            {
                var classOfWork = model.ClassOfWork.Id.ToString();
                var leadCW = lead.Grades.Where(g => g.ClassOfWorkId.ToString() == classOfWork && g.StatusText == "Active").FirstOrDefault();

                if (leadCW != null)
                {
                    string minGradeResult = leadCW.ApprovedGrade;
                    decimal totalAnnualTurnover = 0;
                    decimal totalAvailableCapital = 0;
                    int totalProfessionals = 0;
                    decimal totalLargestContractValue = lead.LargestContractValue;
                    bool hasEELicense = false;
                    bool contractorCount = false;
                    bool anyDuplicates = false;
                    bool contractorLevelBelow = false;
                    bool contractorExpired = false;

                    foreach (ContractorModel result in newModel.Contractors)
                    {
                        if (newModel.Contractors.Count <= 1)
                        {
                            newModel.RecommendedGrade = "Please ensure that more than one Contractor CRS Number is captured for JV Calculation.";
                            contractorCount = true;
                            break;
                        }

                        if (result.StatusText != "Active")
                        {
                            newModel.RecommendedGrade = string.Format("Contractor with CRS Number {0} does not have an Active registration status.", result.CrsNumber);
                            contractorExpired = true;
                            break;
                        }

                        anyDuplicates = newModel.Contractors.Select(i => i.CrsNumber).Distinct().Count() < newModel.Contractors.Count();

                        if (anyDuplicates)
                        {
                            newModel.RecommendedGrade = "Please ensure that no duplicate Contractor CRS Numbers have been captured for JV Calculation.";
                            break;
                        }

                        //Leads Grade in CoW
                        int leadCurrent = Int32.Parse(minGradeResult);
                        int advertisedGrade = GetGradeFromTenderValueRange(model.Designation.Name);

                        //Check that lead Contactors Grade is not more than 1 level lower than Advertised Grade.
                        if (leadCurrent < advertisedGrade - 1)
                        {
                            newModel.RecommendedGrade = string.Format("The Lead Contractors Grade of {0}, is more than 1 level below the selected Advertised Grade. Contractor does not qualify.", leadCurrent);
                            contractorLevelBelow = true;
                            break;
                        }

                        //Current Grade in Cow
                        var resultCW = result.Grades.Where(g => g.ClassOfWorkId.ToString() == classOfWork && g.StatusText == "Active").FirstOrDefault();

                        //check if the contractor is active
                        if (result.StatusText == "Active")
                        {
                            //add annual turnover
                            totalAnnualTurnover += result.AnnualTurnOver;
                            //add available capital
                            totalAvailableCapital += result.AvailableCapital;
                            //add professionals
                            totalProfessionals += result.Professionals;
                            //get the largest contract value of a partner
                            if (result.LargestContractValue > totalLargestContractValue)
                            {
                                totalLargestContractValue = result.LargestContractValue;
                            }
                            //determine if a contractor has an eelicense
                            if (result.EElicense)
                            {
                                hasEELicense = true;
                            }

                            //get the highest grade for class of work
                            var classofworkGrade = result.Grades.FirstOrDefault(g => g.ClassOfWorkId.ToString() == classOfWork && g.StatusText == "Active");

                            if (classofworkGrade != null && !string.IsNullOrWhiteSpace(classofworkGrade.ApprovedGrade))
                            {
                                if (int.Parse(minGradeResult) < int.Parse(classofworkGrade.ApprovedGrade))
                                {
                                    minGradeResult = classofworkGrade.ApprovedGrade;
                                }
                            }
                        }
                    }

                    if (contractorCount != true && anyDuplicates != true && contractorLevelBelow != true && contractorExpired != true)
                    {
                        //get the recommended grade
                        recommendedGrade = await GradingDesignationCalcUtil.GetMethodARecommendedGrade(totalAnnualTurnover, totalLargestContractValue, totalLargestContractValue, totalAvailableCapital, totalProfessionals, hasEELicense, classOfWork);
                        //determine to get the highest grade
                        if ((minGradeResult != null && !string.IsNullOrEmpty(recommendedGrade)) && (int.Parse(recommendedGrade) < int.Parse(minGradeResult)))
                        {
                            recommendedGrade = minGradeResult;
                        }
                        //get the joint venture grade
                        var jvgrade = ProcessGradingDesignation.CalculateGrade(newModel.Contractors);
                        //determine to get the highest grade
                        if ((!string.IsNullOrEmpty(jvgrade) && !string.IsNullOrEmpty(recommendedGrade)) && (int.Parse(recommendedGrade) < int.Parse(jvgrade)))
                        {
                            recommendedGrade = jvgrade;
                        }

                        //TODO COMMENT OUT ON DATE
                        if (int.Parse(recommendedGrade) > int.Parse(minGradeResult) + 1)
                            recommendedGrade = (int.Parse(minGradeResult) + 1).ToString();


                        //BR_06: For Grades 3 and 4, the JV may satisfy either Requirement A OR Requirement C OR both requirements
                        if ((GetGradeFromTenderValueRange(model.Designation.Name) >= 3
                                && GetGradeFromTenderValueRange(model.Designation.Name) <= 4)
                                && !string.IsNullOrEmpty(recommendedGrade))
                        {
                            var grade = GetFinancialGrade(
                                totalAnnualTurnover,
                                totalAvailableCapital);

                            if (grade.HasValue)
                            {
                                recommendedGrade = grade.Value.ToString();
                            }
                        }
                        const string RejectedMessage = "The JV does not meet the lead partner minimum grading requirement.";

                        if (string.IsNullOrEmpty(recommendedGrade))
                        {
                            newModel.RecommendedGrade = "Grade cannot be determined.";
                        }
                        else if (recommendedGrade == RejectedMessage)
                        {
                            newModel.RecommendedGrade = RejectedMessage;
                        }
                        else
                        {
                            newModel.RecommendedGrade = $"The calculated grading and class of work for this JV is {recommendedGrade +
                                newModel.ClassOfWork.Name?.Split(new[] { " - " }, StringSplitOptions.None)[0]}";
                        }

                        newModel.Success = true;
                    }
                    else
                    {
                        newModel.RecommendedGrade = "The lead partner is not currently active for this Class of Construction Works advertised.";
                    }
                }
                else
                {
                    newModel.RecommendedGrade = "Grade cannot be determined.";
                }                
            }

            return newModel;
        }

        public List<ContractorModel> GetJVContractors(JVGradingDesignationModel model, CancellationToken ct = default)
        {
            List<ContractorModel> results = new List<ContractorModel>();

            foreach (ContractorModel crsnumber in model.Contractors)
            {
                if (!string.IsNullOrEmpty(crsnumber.CrsNumber))
                {
                    //
                    var result = GetByCrsNumber(crsnumber.CrsNumber, ct).Result;
                    //check if the contractor is active
                    if (result?.Id == Guid.Empty)
                    {
                        result.StatusText = "Contractor not registered";
                        result.CrsNumber = crsnumber.CrsNumber;
                    }
                    //result.Lead = crsnumber.Lead;
                    results.Add(result);
                }
            }
            return results;
        }

        public async Task<ContractorModel?> GetByCrsNumber(string crsNumber, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.Account)
            {
                ColumnSet = new ColumnSet(true),
                Criteria =
                {
                    Conditions =
                    {
                        new ConditionExpression(CrmFieldNames.ContractorFields.CrsNumber, ConditionOperator.Equal, crsNumber),
                    }
                }
            };

            var entity = await Task.Run(() =>
                _service.RetrieveMultiple(query).Entities.FirstOrDefault(), ct);

            var contractor = entity == null ? null : ContractorMapper.ToDomain(entity);

            contractor?.Grades = GetContractorGrades(contractor.Id).ToList();

            PopulateAnnualTurnoverAndAvailableCapital(contractor);

            PopulateProfessionlasAsync(contractor);

            PopulateLargestContractValue(contractor);

            PopulateEELicense(contractor);

            if (contractor.EElicense)
                contractor.CurrentGradingDesignation = "PE ";

            contractor.CurrentGradingDesignation = contractor.CurrentGradingDesignation + GetCurrentGradingDesignation(contractor);

            return contractor;
        }

        #region Helper Methods
        public void PopulateAnnualTurnoverAndAvailableCapital(ContractorModel result, CancellationToken ct = default)
        {
            var query = new QueryExpression(CrmEntityNames.FinancialStatement)
            {
                ColumnSet = new ColumnSet(true),
                NoLock = true
            };

            query.Criteria.AddCondition(
                CrmFieldNames.FinancialStatementFields.ContractorId,
                ConditionOperator.Equal,
                result.Id);

            query.AddOrder(
                CrmFieldNames.FinancialStatementFields.Year,
                OrderType.Descending);

            var statement = _service.RetrieveMultiple(query).Entities.Select(FinancialStatementMapper.ToDomain).ToList();

            if (statement.Count > 0 && statement[0].Id != Guid.Empty)
            {
                //get the annual turnover
                if (statement[0].TurnoverInclVat > 0 && statement[0].TurnoverInclVat != 0)
                {
                    result.AnnualTurnOver = statement[0].TurnoverInclVat;
                }
                else
                {
                    if (((statement[0].NetAssetValue > 0 && statement[0].NetAssetValue != 0) && (result.CurrentGrade == "3" || result.CurrentGrade == "4")))
                    {
                        result.AnnualTurnOver = CalculateMissingNotionalValue(statement[0].NetAssetValue);
                    }
                }

                //get the available capital
                if (statement[0].NetAssetValue > 0 && statement[0].NetAssetValue != 0)
                {
                    result.AvailableCapital = statement[0].NetAssetValue;
                }
                else
                {
                    if ((statement[0].TurnoverInclVat > 0 && statement[0].TurnoverInclVat != 0) && (result.CurrentGrade == "3" || result.CurrentGrade == "4"))
                    {
                        result.AvailableCapital = CalculateMissingNotionalValue(statement[0].TurnoverInclVat);
                    }
                }
            }
        }

        private void PopulateProfessionlasAsync(ContractorModel result)
        {
            decimal numOfProfessionals = 0;
            var query = new QueryExpression(CrmEntityNames.RegisteredProfessionals)
            {
                NoLock = true,
                Criteria = new FilterExpression(LogicalOperator.And)
            };

            query.Criteria.AddCondition("nv_contractorid", ConditionOperator.Equal, result.Id);

            var professionals = _service.RetrieveMultiple(query).Entities
                .Select(RegisteredProfessionalAllocationMapper.ToDomain)
                .Where(x => x != null).ToList();

            if(professionals.Count > 0)
            {
                int totalDevotedPercentage = 0;
                foreach (var professional in professionals)
                {
                    //
                    if (professional.PercentageWorkingTimeDevotedToEnterprise != null)
                    {
                        totalDevotedPercentage += professional.PercentageWorkingTimeDevotedToEnterprise.Value;
                    }
                }
                //check if the total devoted percentage is not zero
                if (totalDevotedPercentage > 0)
                {
                    numOfProfessionals = decimal.Floor(totalDevotedPercentage / 100);
                }
            }
            result.Professionals = (int)numOfProfessionals;
        }

        private void PopulateLargestContractValue(ContractorModel result)
        {
            var query = new QueryExpression(CrmEntityNames.TrackRecord)
            {
                ColumnSet = new ColumnSet("nv_contractorsshareinclvat"),
                TopCount = 1,
                NoLock = true
            };

            query.Criteria.AddCondition("nv_contractorid", ConditionOperator.Equal, result.Id);
            query.Criteria.AddCondition("statecode", ConditionOperator.Equal, 0);

            query.Orders.Add(new OrderExpression(
                "nv_contractorsshareinclvat",
                OrderType.Descending));

            var foundResult = _service.RetrieveMultiple(query);

            var entity = foundResult.Entities.FirstOrDefault();

            result.LargestContractValue = (decimal)(entity?
                .GetAttributeValue<Money>("nv_contractorsshareinclvat")?
                .Value);
        }

        private void PopulateEELicense(ContractorModel result)
        {
            var query = new QueryExpression(CrmEntityNames.ClassOfWork)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression(LogicalOperator.And),
                TopCount = 1,
                NoLock = true
            };

            query.Criteria.AddCondition("nv_contractorid", ConditionOperator.Equal, result.Id);
            query.Criteria.AddCondition("nv_electricallicense", ConditionOperator.Equal, true);

            result.EElicense = _service.RetrieveMultiple(query).Entities.Any();
        }

        private decimal CalculateMissingNotionalValue(decimal availableValue)
        {
            decimal missingValue = 0;
            //
            decimal num1 = 0;
            //
            decimal num2 = 0;
            //
            num1 = Math.Round(((availableValue - 1000000) / (2000000 - 1000000)), 2);
            //
            num2 = ((200000 - 100000) / 1);
            //
            missingValue = num1 * num2 + 100000;

            return missingValue;

        }

        private string GetCurrentGradingDesignation(ContractorModel contractor)
        {
            var formattedGrades = contractor.Grades
                .Where(g => !string.IsNullOrWhiteSpace(g.ClassOfWorksDescription))
                .OrderBy(g => g.ClassOfWorksDescription)
                .Select(g =>
                {
                    // Expected format: "ME - 9"
                    var parts = g.ClassOfWorksDescription.Split('-');

                    if (parts.Length == 2)
                    {
                        var code = parts[0].Trim(); // "ME"
                        var grade = g.ApprovedGrade?.Trim();

                        return $"{grade}{code}"; // "9ME"
                    }

                    return g.ClassOfWorksDescription;
                })
                .ToList();

            return string.Join(", ", formattedGrades);
        }

        public List<ContractorGradeModel> GetContractorGrades(Guid contractorId)
        {
            var query = new QueryExpression(CrmEntityNames.ClassOfWork)
            {
                ColumnSet = new ColumnSet(
                    ContractorGradeFields.Id,
                    ContractorGradeFields.Name,
                    ContractorGradeFields.ContractorId,
                    ContractorGradeFields.ClassOfWorkTypeId,
                    ContractorGradeFields.ApprovedGrade,
                    ContractorGradeFields.ElectricalLicense,
                    ContractorGradeFields.CreatedOn,
                    ContractorGradeFields.ModifiedOn,
                    ContractorGradeFields.StateCode)
            };

            query.Criteria.AddCondition(
                ContractorGradeFields.ContractorId,
                ConditionOperator.Equal,
                contractorId);

            var entities = _service.RetrieveMultiple(query).Entities;

            return entities
                .Select(ContractorGradeMapper.ToDomain)
                .Where(x => x != null)
                .Cast<ContractorGradeModel>()
                .ToList();
        }

        private int? GetFinancialGrade(decimal totalAnnualTurnover, decimal totalAvailableCapital)
        {
            var matchedGrade = GradeThreeFourRules
                .Where(x =>
                    totalAnnualTurnover >= x.MinimumTurnover
                    || totalAvailableCapital >= x.MinimumAvailableCapital)
                .OrderByDescending(x => x.Grade)
                .FirstOrDefault();

            return matchedGrade?.Grade;
        }

        private static readonly List<GradeDesignationThreeFourRule> GradeThreeFourRules =
        [
            new GradeDesignationThreeFourRule
            {
                Grade = 3,
                MinimumTurnover = 1000000,
                MinimumAvailableCapital = 100000
            },
            new GradeDesignationThreeFourRule
            {
                Grade = 2,
                MinimumTurnover = 2000000,
                MinimumAvailableCapital = 200000
            }
        ];

        private int GetGradeFromTenderValueRange(string tenderValueRange)
        {
            int grade;

            switch (tenderValueRange)
            {
                case "(9) R 200,000,000 – No Limit":
                    grade = 9;
                    break;
                case "(8) R 60, 000,000 – R 200,000,000":
                    grade = 8;
                    break;
                case "(7) R 20,000,000 – R 60,000,000":
                    grade = 7;
                    break;
                case "(6) R 10,000,000 – R 20,000,000":
                    grade = 6;
                    break;
                case "(5) R 6,000,000 – R 10,000,000":
                    grade = 5;
                    break;
                case "(4) R 3,000,000 – R 6,000,000":
                    grade = 4;
                    break;
                case "(3) R 1,000,000 – R 3,000,000":
                    grade = 3;
                    break;
                case "(2) R 200,000 - R 650,000":
                    grade = 2;
                    break;
                default:
                    grade = 0;
                    break;
            }

            return grade;
        }
        #endregion
    }
}
