using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Mindscape.Raygun4Net;
using Mindscape.Raygun4Net.Builders;
using Mindscape.Raygun4Net.Messages;

namespace webapisample.Logging
{
    public class RaygunLogger : ILogger
    {
        private string category;
        private IHttpContextAccessor httpAccessor;
        private RaygunSettings raygunSettings;
        private IHostingEnvironment hostingEnvironment;

        public RaygunLogger(string category, IHttpContextAccessor httpAccessor, RaygunSettings raygunSettings,
            IHostingEnvironment hostingEnvironment)
        {
            this.category = category;
            this.httpAccessor = httpAccessor;
            this.raygunSettings = raygunSettings;
            this.hostingEnvironment = hostingEnvironment;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= LogLevel.Error;
        }

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel))
            {
                return;
            }

            var client = new RaygunClient(this.raygunSettings);
                        var raygunMessageBuilder = RaygunAspNetCoreMessageBuilder.New(this.raygunSettings);

            var httpContext = this.httpAccessor.HttpContext;
            if (httpContext != null)
            {
                var emptyList =  new string[0];
                var messageOptions = new RaygunRequestMessageOptions(
                    this.raygunSettings.IgnoreFormFieldNames ?? emptyList,
                    this.raygunSettings.IgnoreHeaderNames ?? emptyList,
                    this.raygunSettings.IgnoreCookieNames ?? emptyList,
                    this.raygunSettings.IgnoreServerVariableNames ?? emptyList);

                messageOptions.IsRawDataIgnored = this.raygunSettings.IsRawDataIgnored;

                var requestMessage = await RaygunAspNetCoreRequestMessageBuilder.Build(httpContext, messageOptions);
                raygunMessageBuilder.SetRequestDetails(requestMessage);
            }

            IList<string> tags = new List<string>
            {
                this.hostingEnvironment.ApplicationName,
                this.hostingEnvironment.EnvironmentName
            };

            raygunMessageBuilder
                .SetExceptionDetails(exception)
                .SetClientDetails()
                .SetEnvironmentDetails()
                .SetTags(tags)
                .SetMachineName(Environment.MachineName)
                .SetVersion(this.raygunSettings.ApplicationVersion);
                //.SetUserCustomData(FilterRenderedMessageInUserCustomData(userCustomData, renderedMessageFilter));

            RaygunMessage raygunMessage = raygunMessageBuilder.Build();
            string message = formatter(state, exception);

            if (exception == null)
            {
                raygunMessage.Details.Error = new RaygunErrorMessage
                {
                    Message = message,
                    ClassName = this.category
                };
            }
            else if (!string.IsNullOrWhiteSpace(message))
            {
                raygunMessage.Details.Error.Message = $"{message} ({raygunMessage.Details.Error.Message})";
            }

            await client.SendInBackground(raygunMessage);
        }
    }
}