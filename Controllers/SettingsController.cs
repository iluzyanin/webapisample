using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
        public ApplicationSettings Get()
        {
            return settings;
        }
    }
}