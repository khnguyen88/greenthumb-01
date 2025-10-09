using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgenticGreenthumbApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdafruitController : ControllerBase
    {
        private readonly ILogger<AdafruitController> _logger;
        private readonly AdafruitService _adafruitService;

        public AdafruitController(ILogger<AdafruitController> logger, AdafruitService adafruitService)
        {
            _logger = logger;
            _adafruitService = adafruitService;
        }

        [HttpGet("GetHumidityFeedData", Name = "GetHumidityFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetHumidityFeedData()
        {
            return Ok(_adafruitService.GetHumidityFeedData().Result);
        }
    }
}
