using iTender.Domain.Models;
using Microsoft.Xrm.Sdk;
using System.Globalization;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Infrastructure.Mappers
{
    public static class ContractorDevelopmentProgrammeMapper
    {
        public static ContractorDevelopmentProgrammeModel ToDomain(Entity entity)
        {
            var model = new ContractorDevelopmentProgrammeModel();

            model.Id = entity.Id;

            var employerRef = entity.GetAttributeValue<EntityReference>(ContractorDevelopmentProgrammeFields.EmployerId);

            if (employerRef != null)
            {
                model.EmployerId = employerRef.Id;
                model.EmployerName = employerRef.Name;
            }

            model.CDPNumber = entity.GetAttributeValue<string>(ContractorDevelopmentProgrammeFields.CDPNumber);

            var cdpName = entity.GetAttributeValue<string>(ContractorDevelopmentProgrammeFields.Name);

            if (!string.IsNullOrWhiteSpace(cdpName))
            {
                model.CDPName = cdpName.Length > 5
                    ? cdpName.Remove(0, 5)
                    : cdpName;
            }

            var primaryFocus = entity.GetAttributeValue<OptionSetValue>(ContractorDevelopmentProgrammeFields.PrimaryFocus);

            if (primaryFocus != null)
            {
                model.PrimaryFocus = primaryFocus.Value;

                model.PrimaryFocusText = GetOptionSetLabel(entity, ContractorDevelopmentProgrammeFields.PrimaryFocus);
            }

            var totalBudget = entity.GetAttributeValue<Money>("nv_totalbudget");

            if (totalBudget != null)
            {
                decimal rands = Math.Truncate(totalBudget.Value);
                model.TotalBudgetAmountRands = rands.ToString();

                var totalBudgetAmount = totalBudget.Value.ToString(
                    "C",
                    new CultureInfo("en-ZA")
                    {
                        NumberFormat = new NumberFormatInfo
                        {
                            CurrencySymbol = "R",
                            CurrencyDecimalSeparator = ".",
                            CurrencyGroupSeparator = ",",
                            NumberDecimalSeparator = ".",
                            NumberGroupSeparator = ","
                        }
                    });

                model.TotalBudgetAmount = totalBudgetAmount;

                model.TotalBudgetAmountCents =
                    totalBudgetAmount.Contains(".")
                        ? totalBudgetAmount[(totalBudgetAmount.IndexOf('.') + 1)..]
                        : "00";
            }

            return model;
        }

        private static string GetOptionSetLabel(
            Entity entity,
            string attributeName)
        {
            if (entity.FormattedValues.Contains(attributeName))
            {
                return entity.FormattedValues[attributeName];
            }

            return string.Empty;
        }
    }
}
