using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System;
using System.Collections.Generic;
using webapisample.Models;

namespace webapisample.Controllers
{
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly ApplicationSettings settings;

        public PostsController(IOptionsSnapshot<ApplicationSettings> settingsOptions)
        {
            settings = settingsOptions.Value;
        }

        [HttpGet]
        public IList<Post> Get()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(settings.ServiceUrl)
            };

            var response = client.GetAsync("posts").Result;
            if (!response.IsSuccessStatusCode)
            {
                return new List<Post>()
                {
                    new Post { Title= settings.ServiceUrl, Body = response.ReasonPhrase }
                };
            }
            var content = response.Content.ReadAsAsync<List<Post>>();

            return content.Result;
        }
    }
}