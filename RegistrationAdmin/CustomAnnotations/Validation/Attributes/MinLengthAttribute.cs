using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using RegistrationAdmin.Helpers;
using RegistrationAdmin.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAdmin.CustomAnnotations.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum)]
    public class MinLength : MinLengthAttribute
    {
        public string SummaryErrorMessage { get; set; }

        private readonly int _minLength;

        public MinLength(int minLength, string summaryErrorMessage = null) : base(minLength)
        {
            SummaryErrorMessage = summaryErrorMessage;
            _minLength = minLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            if (value is string stringValue)
            {
                //string
                value = stringValue.Trim().ReplaceCarriageReturnAndLineFeedWithCarriageReturn();
            }

            if (value is TrimString trimValue)
            {
                value = (string)trimValue;
            }

            if (!IsValid(value))
            {
                validationContext.AddValidationSummary(SummaryErrorMessage);
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }
}
