using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System;
using System.Collections.Generic;

namespace webapisample.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private readonly ApplicationSettings settings;

        public SettingsController(IOptions<ApplicationSettings> settingsOptions)
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
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = client.GetAsync("").Result;
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

    public class Post
    {
        public int UserId { get; set; }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }
    }
}