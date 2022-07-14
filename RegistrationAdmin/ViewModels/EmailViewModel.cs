using System.ComponentModel;

using RegistrationAdmin.CustomAnnotations.Validation;
using RegistrationAdmin.CustomAnnotations.Validation.Attributes;
using RegistrationAdmin.Models.Constants;


namespace RegistrationAdmin.ViewModels
{
    public class EmailViewModel
    {
        [@Required(ErrorMessage = "Please enter a valid email", SummaryErrorMessage = "Please enter a valid email")]
        [RegularExpression(TextPattern.Email, SummaryErrorMessage = "Please enter a valid email address", ErrorMessage = "Enter a valid email")]
        [MinLength(6, SummaryErrorMessage = "Email is too short", ErrorMessage = "Email must be greater than 6 characters")]
        [MaxLength(100, SummaryErrorMessage = "Email - your email is too long", ErrorMessage = "Email must be 6 to 100 characters")]
        [DisplayName("Email address")]
        public string EmailAddress { get; set; }
        public string ConfirmationMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
