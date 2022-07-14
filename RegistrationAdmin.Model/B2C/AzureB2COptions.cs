
namespace RegistrationAdmin.Models.B2C
{
    public interface IAzureB2COptions
    {
        string B2CTenant { get; set; }
        string B2CClientId { get; set; }
        string B2CClientSecret { get; set; }
    }
    
    public class AzureB2COptions: IAzureB2COptions
    {
        public string B2CTenant { get; set; }
        public string B2CClientId { get; set; }
        public string B2CClientSecret { get; set; }      

        }
    }

