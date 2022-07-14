using System.Net.Http;
using System.Threading.Tasks;
using RegistrationAdmin.Models.B2C;

namespace AzureB2CGraphService.Interfaces
{
    public interface IB2CGraphService
    {
      Task<HttpResponseMessage> CreateUser(string user, IAzureB2COptions _AzureB2COptions);
      Task<string> UserExist(string useremail, IAzureB2COptions _AzureB2COptions);
    }
}
