using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAdmin.CustomAnnotations.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum)]
    public class RegularExpression : RegularExpressionAttribute
    {
        public string SummaryErrorMessage { get; set; }
        public RegularExpression(string pattern, string summaryErrorMessage = null) : base(pattern)
        {
            SummaryErrorMessage = summaryErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var isValid = base.IsValid(value, context);
            if (isValid != ValidationResult.Success)
            {
                context.AddValidationSummary(SummaryErrorMessage);
            }

            return isValid;
        }
    }
}