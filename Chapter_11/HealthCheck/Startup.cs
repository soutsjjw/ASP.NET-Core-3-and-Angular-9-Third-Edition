using HealthCheck.Settings.StaticFiles;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HealthCheck
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public Models.StaticFiles.Headers Headers { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddHealthChecks();

            services.AddStaticFilesHeader(Configuration);

            services.AddHealthChecks()
                .AddCheck("ICMP_01", new ICMPHealthCheck("www.ryadel.com", 100))
                .AddCheck("ICMP_02", new ICMPHealthCheck("www.google.com", 100))
                .AddCheck("ICMP_03", new ICMPHealthCheck("www.does-not-exist.com", 100))
                ;

            var provider = services.BuildServiceProvider();
            Headers = provider.GetService<Models.StaticFiles.Headers>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            // add .webmanifest MIME-type support
            FileExtensionContentTypeProvider provider = new FileExtensionContentTypeProvider();
            provider.Mappings[".webmanifest"] = "application/manifest+json";

            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = provider,

                OnPrepareResponse = (context) =>
                {
                    // Disable caching for all static files.
                    // Retrieve cache configuration from appsettings.json
                    context.Context.Response.Headers["Cache-Control"] = Headers.Cache_Control;
                    context.Context.Response.Headers["Pragma"] = Headers.Pragma;
                    context.Context.Response.Headers["Expires"] = Headers.Expires;
                }
            });
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles(new StaticFileOptions()
                {
                    ContentTypeProvider = provider
                });
            }

            app.UseRouting();

            app.UseHealthChecks("/hc", new CustomHealthCheckOptions());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    //spa.UseAngularCliServer(npmScript: "build");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
