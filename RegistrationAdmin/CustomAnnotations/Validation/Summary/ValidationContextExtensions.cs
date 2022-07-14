using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;


namespace RegistrationAdmin.CustomAnnotations.Validation.Summary
{
    public static class ValidationContextExtensions
    {
        public static void AddValidationSummary(this ValidationContext context, string msg)
        {
            var summaryService = context.GetService<IValidationSummaryService>();
            if(summaryService != null)
            {
                summaryService.AddMessage(context, msg);
            }
        }

      
    }
}