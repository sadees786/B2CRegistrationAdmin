using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegistrationAdmin.CustomAnnotations.Validation.Summary
{
    public interface IValidationSummaryService
    {
        void AddMessage(ValidationContext context, string msg);

        void AddMessage(ValidationMessage msg);

        IList<ValidationMessage> GetSummary();
        
        void ClearMessages();
    }
}