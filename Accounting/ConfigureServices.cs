using Common;
using Microsoft.AspNetCore.SignalR;
using Service.Infrastructure.CustomMapping;
using WebFramework.Configuration;

namespace Api
{
    public static class ConfigureServices
    {
        public static void AddWebServices(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSettings)
        {
            services.AddMinimalMvc();
            services.AddRazorPages();
            services.AddCors(option =>
            {
                option.AddPolicy(name: "PublishedOrigins", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(
                            "http://app.1000bar.ir",
                            "https://app.1000bar.ir",
                            "http://m.1000bar.ir",
                            "https://m.1000bar.ir",
                            "http://pwa.1000bar.ir",
                            "https://pwa.1000bar.ir")
                        .AllowCredentials();
                });

                option.AddPolicy(name: "SahandOrigins", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(
                            "http://1000barnew.sk.local",
                            "https://1000barnew.sk.local",
                            "http://m.1000barnew.sk.local",
                            "https://m.1000barnew.sk.local",
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "http://localhost:4201",
                            "https://localhost:4201",
                            "https://localhost:5000")
                        .AllowCredentials();
                });

                option.AddPolicy(name: "DevelopedOrigins", policyBuilder =>
                {
                    policyBuilder
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithOrigins(
                            "http://localhost:4200",
                            "https://localhost:4200",
                            "http://localhost:4201",
                            "https://localhost:4201")
                        .AllowCredentials();
                });

            });
            services.AddOptions();
            services.AddLazyResolution();
            services.AddMemoryCache();
   
            // services.AddMiniProfiler();
    
            services.AddCustomApiVersioning();
            // services.AddSignalR();
            // services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSettings)
        {
            services.InitializeAutoMapper();
            services.AddDbContext(configuration);
            services.AddCustomIdentity(siteSettings.IdentitySettings);
            services.AddJwtAuthentication(siteSettings.JwtSettings);
        }

        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration, SiteSettings siteSettings)
        {
            services.Configure<SiteSettings>(configuration.GetSection(nameof(SiteSettings)));
            
            services.ConfigureServices();
        }
    }
}