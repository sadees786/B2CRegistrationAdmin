using RegistrationAdmin.CustomAnnotations.Validation;
using System.ComponentModel;
using RegistrationAdmin.CustomAnnotations.Validation.Attributes;

namespace RegistrationAdmin.ViewModels
{
    public class DownloadFileViewModel
    {
        [@Required(ErrorMessage = "Please enter a reference number", SummaryErrorMessage = "Please enter a reference number")]
        [DisplayName("Application reference number")]
        [MaxLength(20,"Please enter correct application reference number")]
        public string ReferencNumber { get; set; }
        public string ErrorMessage { get; set; } = "The file cannot be found. Please check the application reference number and try again";
        public string SourceSystem { get; set; }
    }
}
