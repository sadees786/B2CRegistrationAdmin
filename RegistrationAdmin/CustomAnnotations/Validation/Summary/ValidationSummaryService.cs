using MoreLinq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RegistrationAdmin.CustomAnnotations.Validation.Summary
{
    public class ValidationSummaryService : IValidationSummaryService
    {
        private readonly IList<ValidationMessage> _messages = new List<ValidationMessage>();

        public void AddMessage(ValidationMessage msg)
        {
            _messages.Add(msg);
        }

        public void AddMessage(ValidationContext context, string msg)
        {
            var propName = context.MemberName;
            AddMessage(new ValidationMessage(propName, msg));
        }

        public IList<ValidationMessage> GetSummary()
        {
            return _messages.DistinctBy(m => m.PropertyName).ToList();
        }
        public void ClearMessages()
        {
            _messages.Clear();
        }
    }
}
