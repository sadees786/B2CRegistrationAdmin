using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAdmin.GovNotify
{
    public interface IGovNotifySettings<T> where T : class
    {
        string Endpoint { get; set; }
        string Key { get; set; }
    }

    public class GovNotifySettings<T> : IGovNotifySettings<T> where T : class
    {
        public string Endpoint { get; set; }
        public string Key { get; set; }
    }

    public interface IGovUkNotifyConfiguration
    {
        string InviteEmailTemplateId { get; set; }
        string ResetPasswordTemplateId { get; set; }
        string ReplyEmailTemplateId { get; set; }
    }

    public class GovUkNotifyConfiguration : IGovUkNotifyConfiguration
    {
        public string InviteEmailTemplateId { get; set; }
        public string ResetPasswordTemplateId { get; set; }
        public string ReplyEmailTemplateId  { get; set; } 
    }
}
