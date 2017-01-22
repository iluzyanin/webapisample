using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mindscape.Raygun4Net;

namespace webapisample.Logging
{
    public class RaygunLoggerProvider : ILoggerProvider
    {
        private IHttpContextAccessor httpAccessor;
        private RaygunSettings raygunSettings;
        private IHostingEnvironment hostingEnvironment;

        public RaygunLoggerProvider(IHttpContextAccessor httpAccessor, RaygunSettings raygunSettings,
            IHostingEnvironment hostingEnvironment)
        {
            this.httpAccessor = httpAccessor;
            this.raygunSettings = raygunSettings;
            this.hostingEnvironment = hostingEnvironment;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new RaygunLogger(categoryName, this.httpAccessor, this.raygunSettings, this.hostingEnvironment);
        }

        public void Dispose()
        { }
    }
}