using HealthCheck.Models.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HealthCheck.Settings.StaticFiles
{
    public class HeadersSettings
    {
        public string Cache_Control { get; set; }
        public string Pragma { get; set; }
        public string Expires { get; set; }
    }

    public static class HeadersExtensions
    {
        public static IServiceCollection AddStaticFilesHeader(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("StaticFiles:Headers");
            // we first need to create an instance
            var settings = new HeadersSettings();
            // then we set the properties 
            new ConfigureFromConfigurationOptions<HeadersSettings>(section)
                .Configure(settings);
            // then we register the instance into the services collection
            services.AddSingleton(new Headers(settings));

            return services;
        }
    }
}
