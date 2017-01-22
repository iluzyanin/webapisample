using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mindscape.Raygun4Net;

namespace webapisample.Logging
{
    public static class RaygunLoggerFactoryExtensions
    {
        public static void AddRaygun(this ILoggerFactory loggerFactory,
            IHttpContextAccessor httpAccessor,
            IOptions<RaygunSettings> raygunOptions,
            IHostingEnvironment hostingEnvironment)
        {
            loggerFactory.AddProvider(new RaygunLoggerProvider(httpAccessor, raygunOptions.Value, hostingEnvironment));
        }
    }
}