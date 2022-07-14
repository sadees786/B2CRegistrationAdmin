namespace AzureB2CGraphService.Model
{
    public class AuthenticationConfig
    {
        /// <summary>
        /// The Tenant is tenant ID of the Azure AD in which this application is registered (a guid)
        /// </summary>
        public string B2CTenant { get; set; }
        /// <summary>
        /// Guid used by the application to uniquely identify itself to Azure AD
        /// </summary>
        public string B2CClientId { get; set; }   
        /// <summary>
        /// Client secret (application password)
        /// </summary>     
        public string B2CClientSecret { get; set; }

    
    }
}

