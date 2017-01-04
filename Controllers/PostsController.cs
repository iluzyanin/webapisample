using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using System.Collections.Generic;
using webapisample.Models;

namespace webapisample.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private ILogger<PostsController> logger;

        private readonly ApplicationSettings settings;

        public PostsController(IOptionsSnapshot<ApplicationSettings> settingsOptions, ILogger<PostsController> logger)
        {
            this.settings = settingsOptions.Value;
            this.logger = logger;
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
    }
}