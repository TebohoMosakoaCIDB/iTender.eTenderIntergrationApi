using iTender.Application.DTOs;
using iTender.Application.Interfaces;
using iTender.Domain.Business.Rules;

namespace iTender.Application.Utilities
{
    public class GradingDesignationCalcUtil
    {
        private readonly ILookupService _lookupService;
        private readonly IGradingRepository _gradingRepository;

        public GradingDesignationCalcUtil(ILookupService lookupService, IGradingRepository gradingRepository)
        {
            _lookupService = lookupService;
            _gradingRepository = gradingRepository;
        }

        public async Task<RecommendedGradeModel> GetRecommendedGradeAsync(
       GradingDesignationCalculatorModel model)
        {
            var classOfWork = _lookupService.GetClassOfConstructionWorkByIdAsync(model.ClassOfConstructionWorksId).Result?.Name;

            string grade = await GetMethodARecommendedGrade(
                model.BestAnnualTurnover,
                model.LargestContractValue,
                model.ContractValue,
                model.AvailableCapital,
                model.NumberOfQualifiedProfessionals,
                model.ElectricalContractorsLicense,
                classOfWork);

            if (classOfWork == "EB" && !model.ElectricalContractorsLicense)
                grade = "1";

            var result = new RecommendedGradeModel
            {
                Grade = grade
            };

            if (string.IsNullOrEmpty(grade))
                return result;

            var grading = await _gradingRepository.GetByGradeAsync(grade);

            if (grading == null)
                return result;

            result.TenderFromValue = grading.TenderFromValue;
            result.TenderToValue = grading.TenderToValue;
            result.AdminFee = grading.AdminFee;
            result.AnnualFee = grading.AnnualFee;
            result.RegFee = grading.AdminFee.Value + grading.AnnualFee.Value;
            result.Message = $"{grading.TenderFromValue:C} - {grading.TenderToValue:C} (Designation {grading.Grade}) (Registration Fee: {result.RegFee:C})";

            return result;
        }

        public static async Task<string> GetMethodARecommendedGrade(
            decimal annualTurnover,
            decimal largestContractValue,
            decimal contractValue,
            decimal availableCapital,
            int professionals,
            bool eelicense,
            string classOfWork)
        {
            IGradingContextFactory contextFactory =
                new OfflineGradingContextFactory(
                    annualTurnover,
                    largestContractValue,
                    contractValue,
                    availableCapital,
                    professionals,
                    eelicense,
                    classOfWork,
                    DateTime.Today);

            var calculator = new GradingCalculator(contextFactory);

            var recommendedGrades =
                await calculator.CalculateRecommendedGrades(Guid.NewGuid()); // ✅ FIX

            return recommendedGrades
                .MethodARecommendedGrade
                .HighestGradeAchieved
                .Value
                .ToString();
        }
    }
}
