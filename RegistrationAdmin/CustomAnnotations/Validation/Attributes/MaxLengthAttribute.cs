using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using RegistrationAdmin.Helpers;
using System;
using System.ComponentModel.DataAnnotations;


namespace RegistrationAdmin.CustomAnnotations.Validation.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Enum)]
    public class MaxLength : MaxLengthAttribute
    {
        public string SummaryErrorMessage { get; set; }

        private readonly int _maxLength;

        public MaxLength(int maxLength, string summaryErrorMessage = null) : base(maxLength)
        {
            _maxLength = maxLength;
            SummaryErrorMessage = summaryErrorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()) && value.ToString().ReplaceCarriageReturnAndLineFeedWithCarriageReturn().Length > _maxLength)
            {
                context.AddValidationSummary(SummaryErrorMessage);
                return new ValidationResult(FormatErrorMessage(context.DisplayName));
            }
            return ValidationResult.Success;
        }
    }
}
