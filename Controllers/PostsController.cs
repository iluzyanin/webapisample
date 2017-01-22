using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Collections.Generic;
using webapisample.Models;
using Mindscape.Raygun4Net;

namespace webapisample.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private ILogger<PostsController> logger;
        private readonly ApplicationSettings settings;

        private IExceptionThrower exceptionThrower;

        public PostsController(IOptionsSnapshot<ApplicationSettings> settingsOptions, ILogger<PostsController> logger,
            IExceptionThrower exceptionThrower)
        {
            this.settings = settingsOptions.Value;
            this.logger = logger;
            this.exceptionThrower = exceptionThrower;
        }

        [HttpGet]
        public IList<Post> Get()
        {
            logger.LogInformation("Getting posts");
            var client = new HttpClient
            {
                BaseAddress = new Uri(settings.ServiceUrl)
            };

            var response = client.GetAsync("posts").Result;
            if (!response.IsSuccessStatusCode)
            {
                logger.LogError($"Could not get posts: {(int)response.StatusCode}, {response.ReasonPhrase}");
                return new List<Post>();
            }
            var content = response.Content.ReadAsAsync<List<Post>>();

            return content.Result;
        }

        [Route("error")]
        [HttpGet]
        public void Error()
        {
            // Case 1: Error without exception with parameterized message
            // logger.LogError("Error without exception, param1: {p1}, param2: {p2}", 123, "Some string");

            // Case 2: Error without exception with complex parameter
            // logger.LogError("Error without exception, param1: {p1}, param2: {p2}", 123, "Some string");

            // try
            // {
            //     this.exceptionThrower.ThrowNotImplementedException();
            // }
            // catch (NotImplementedException ex)
            // {
            //     // Case 3: Error with Exception without message
            //     logger.LogError(0, ex, null);
            // }

            try
            {
                this.exceptionThrower.ThrowNotImplementedException();
            }
            catch (NotImplementedException ex)
            {
                // Case 4: Error with both Exception and parameterized message 
                logger.LogError(0, ex,
                    "Error with exception and message param1: {p1}, param2: {p2}", 123, "Some string");
            }
            return;

            // Case 5: Throwing exception
            throw new Exception("Hello, world!");
        }
    }
}