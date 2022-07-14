using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using RegistrationAdmin.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAdmin.CustomAnnotations.Validation

{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum)]
    public class Required : RequiredAttribute
    {
        public string SummaryErrorMessage { get; set; }

        public Required(string summaryErrorMessage = null)
        {
            SummaryErrorMessage = summaryErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            var isValid = IsValid(value);
            if (!isValid)
            {
                context.AddValidationSummary(SummaryErrorMessage);
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }

            return ValidationResult.Success;
        }

        public override bool IsValid(object value)
        {
            if (value is IEmptyCheck emptyCheckValue)
            {
                return !emptyCheckValue.IsEmpty();
            }
            return base.IsValid(value);
        }
    }
}