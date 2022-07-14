using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using RegistrationAdmin.Authorization;
using RegistrationAdmin.Authorization.Handlers;
using RegistrationAdmin.CustomAnnotations.Validation.Summary;
using RegistrationAdmin.Data.DBContext;
using RegistrationAdmin.GovNotify;
using RegistrationAdmin.Models.Application;
using RegistrationAdmin.Models.B2C;
using RegistrationAdmin.Models.FileDownLoad;
using RegistrationAdmin.Service;
using Serilog;
using System.Collections.Generic;


namespace RegistrationAdmin
{
    //TODO: Need to refactor this file

    public class Startup
    {
        private static AppSettings AppSettings { get; } = new AppSettings();
        public IConfiguration Configuration { get; set; }
        private IHostingEnvironment CurrentEnvironment { get; set; }

        private bool IsLocalEnv() => (CurrentEnvironment.IsEnvironment("Local") || CurrentEnvironment.IsEnvironment("local"));

        public Startup(IHostingEnvironment env)
        {
            CurrentEnvironment = env;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (IsLocalEnv())
            {

                builder.AddUserSecrets<Startup>();
            }

            var builtConfig = builder.Build();


            if (!IsLocalEnv())
            {
                var azureVaultUrl = $"{builtConfig["KeyVault:Url"]}";
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                builder.AddAzureKeyVault(azureVaultUrl, keyVaultClient, new DefaultKeyVaultSecretManager());

                // Build and add the Log Database connection from key vault values
                builtConfig = builder.Build();
                var logConfig = new LogSettings();
                builtConfig.Bind("LogSettings", logConfig);

                var appSettings = new AppSettings();
                builtConfig.Bind("AppSettings", appSettings);
                
                builder.AddInMemoryCollection(new Dictionary<string, string> { { "ConnectionStrings:loggingDbConnection", logConfig.GetLoggingDatabaseConnectionString() } });
            }
            Configuration = builder.Build();
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RegistrationAdminLogsContext>(options =>
                                                                    options.UseSqlServer(
                                                                                         Configuration.GetConnectionString("LoggingConnectiondb")));

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(Configuration).CreateLogger(); 

            services.Configure<CookiePolicyOptions>(options =>
                                                    {
                                                        // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                                                        options.CheckConsentNeeded = context => true;
                                                        options.MinimumSameSitePolicy = SameSiteMode.None;
                                                    });
            services.AddSession();

            if (!IsLocalEnv())
            {

                services.AddAuthentication(AzureADDefaults.AuthenticationScheme)
                    .AddAzureAD(options => Configuration.Bind("AzureAd", options));
            }

            services.AddAuthorization(options =>
                {
                    var azureGroupConfig = new List<AzureGroupConfig>();
                    Configuration.Bind("AzureAdGroups", azureGroupConfig);
                    foreach (var adGroup in azureGroupConfig)
                        options.AddPolicy(adGroup.GroupName, policy => policy.AddRequirements(new IsMemberOfGroup(adGroup.GroupName, adGroup.GroupId)));
                    options.AddPolicy("GroupsCheck", policy => policy.Requirements.Add(new GroupsCheckRequirement(azureGroupConfig)));
                });

            services.AddSingleton(AppSettings);


            if (!IsLocalEnv())
            {
                services.AddSingleton<IAuthorizationHandler, IsMemberOfGroupHandler>();
            }
            else
            {
                services.AddSingleton<IAuthorizationHandler, IsMemberOfLocalUserHandler>();
            }

            
            services.AddHttpClient<IDownloadDocumentClient, DownloadDocumentClient>();
            services.AddSingleton<IAuthorizationHandler, GroupCheckHandler>();

        //   services.AddScoped<INotificationService, NotificationService.NotificationService>();
            services.AddScoped<IValidationSummaryService, ValidationSummaryService>();
            services.TryAddSingleton<IDocumentSettings>(_ => Configuration.GetSection("DocumentSettings").Get<DocumentSettings>());
            services.TryAddSingleton<IAzureB2COptions>(_ => Configuration.GetSection("AzureB2C").Get<AzureB2COptions>());
            services.TryAddSingleton<IAppSettings>(_ => Configuration.GetSection("AppSettings").Get<AppSettings>());
            services.TryAddSingleton<IGovUkNotifyConfiguration>(_ => Configuration.GetSection("GovUkNotify").Get<GovUkNotifyConfiguration>());
        //    services.TryAddSingleton<IAsyncNotificationClient>(_ => new NotificationClient(Configuration.GetConnectionString("GovUkNotifyApiKey")));
            var loggingDbConnection = Configuration.GetConnectionString("LoggingDBConnection");
            services.AddDbContext<RegistrationAdminLogsContext>(options => options.UseSqlServer(loggingDbConnection));

            if (!IsLocalEnv())
            {
                services.AddMvc(options =>
                                {
                                    var policy = new AuthorizationPolicyBuilder()
                                                .RequireAuthenticatedUser()
                                                .Build();
                                    options.Filters.Add(new AuthorizeFilter(policy));
                                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            }
            else
            {

                services.AddMvc(options =>
                                {
                                    var policy = new AllowAnonymousFilter();
                                    options.Filters.Add(policy);
                                }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment() || IsLocalEnv())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Home}/{id?}");
            });
        }
    }
}
