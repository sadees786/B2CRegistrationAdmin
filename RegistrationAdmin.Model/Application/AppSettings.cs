namespace RegistrationAdmin.Models.Application
{
    public interface IAppSettings
    {
        string AdminAppUrl { get; set; }
        string RegAppUrl { get; set; }
        
    }
    public class AppSettings : IAppSettings
    {
        public string AdminAppUrl { get; set; }
        public string RegAppUrl { get; set; }  
    }
}