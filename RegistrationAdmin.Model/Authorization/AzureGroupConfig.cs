
namespace RegistrationAdmin.Models.B2C
{
    public interface IAzureGroupConfig
    {
      string GroupId { get; set; }
      string GroupName { get; set; }
    }  
    public class AzureGroupConfig : IAzureGroupConfig
    {
       public string GroupId { get; set; }
       public string GroupName { get; set; }
    }
}

