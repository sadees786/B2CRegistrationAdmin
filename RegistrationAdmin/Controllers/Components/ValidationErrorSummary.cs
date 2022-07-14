using Microsoft.AspNetCore.Mvc;
using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using System.Threading.Tasks;

namespace RegistrationAdmin.Controllers.Components
{
    public class ValidationErrorSummary : ViewComponent
    {
        private readonly IValidationSummaryService _validationService;

        public ValidationErrorSummary(IValidationSummaryService validationService)
        {
            _validationService = validationService;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            var errorSummaryMessages = _validationService.GetSummary();
            return Task.FromResult<IViewComponentResult>(View("ErrorSummary", errorSummaryMessages));
        }
    }
}