using System.Net.Http;
using System.Threading.Tasks;
using AzureB2CGraphService.Model;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using RegistrationAdmin.Models.B2C;

namespace AzureB2CGraphService
{

    public class B2CGraphClient
    {
        private IAzureB2COptions _azureOptions;
        private readonly AuthenticationContext _authContext;
        private readonly ClientCredential _credential;
        public B2CGraphClient(IAzureB2COptions azureOption)
        {
            _azureOptions = azureOption;
            _authContext = new AuthenticationContext(Globals.AadInstance + _azureOptions.B2CTenant);
            _credential = new ClientCredential(_azureOptions.B2CClientId, _azureOptions.B2CClientSecret);
        }

        public async Task<HttpResponseMessage> CreateUser(string api,string query)
        {
            var result = await _authContext.AcquireTokenAsync(Globals.AadGraphEndpoint, _credential);
            var helper = new Helper.Helper(new HttpClient());
            var url = Globals.AadGraphEndpoint + _azureOptions.B2CTenant + api + "?" + Globals.AadGraphVersion;
            var response = await helper.CallWebApiPost(url, result.AccessToken, query);
            return response;           
        }

        public async Task<string> UserExist(string api, string query)
        {
            var result = await _authContext.AcquireTokenAsync(Globals.AadGraphEndpoint, _credential);
            var helper = new Helper.Helper(new HttpClient());      
            string url = Globals.AadGraphEndpoint + _azureOptions.B2CTenant + api + "?$filter=signInNames/any(x:x/value" + " " + "eq " +"'" + query + "'" + ")&" + Globals.AadGraphVersion;
            var response = await helper.CallWebApiGet(url, result.AccessToken);
            return response;
        }

    }
}