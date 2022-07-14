namespace RegistrationAdmin.CustomAnnotations.Validation.Summary
{
    public class ValidationMessage
    {
        public ValidationMessage(string propertyName, string msg)
        {
            PropertyName = propertyName;
            Message = msg;
        }

        public string Message { get; }

        public string PropertyName { get; }
    }
}