
using System.Net.Http;
using System.Threading.Tasks;
using AzureB2CGraphService.Interfaces;
using RegistrationAdmin.Models.B2C;

namespace AzureB2CGraphService
{
    public class B2CGraphService : IB2CGraphService
    { 
        
        public async Task<HttpResponseMessage> CreateUser(string user, IAzureB2COptions _AzureB2COptions)
        {
            B2CGraphClient _client = new B2CGraphClient(_AzureB2COptions);
            var result = await _client.CreateUser("/users/", user);
            return result;
        }

        public async Task<string> UserExist(string useremail, IAzureB2COptions _AzureB2COptions)
        {
            B2CGraphClient _client = new B2CGraphClient(_AzureB2COptions);
            var result = await _client.UserExist("/users/", useremail);
            return result;
        }
    }
}
