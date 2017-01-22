using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;
using webapisample.Logging;
using System.IO;
using Mindscape.Raygun4Net;

namespace webapisample
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<ApplicationSettings>(Configuration);

            // Add framework services.
            services.AddMvc();

            services.AddRaygun(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<IExceptionThrower, ExceptionThrower>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddRaygun(app.ApplicationServices.GetService<IHttpContextAccessor>(),
                app.ApplicationServices.GetService<IOptions<RaygunSettings>>(), env);

            app.UseMvc();
        }
    }
}
