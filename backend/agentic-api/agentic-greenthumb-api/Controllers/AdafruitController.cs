using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AgenticGreenthumbApi.Controllers
{
    [EnableCors("AdafruitPolicy")]
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

        [HttpGet("GetGrowLightFeedData", Name = "GetGrowLightFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetGrowLightFeedData()
        {
            return Ok(_adafruitService.GetGrowLightFeedData().Result);
        }

        [HttpGet("GetHumidityFeedData", Name = "GetHumidityFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetHumidityFeedData()
        {
            return Ok(_adafruitService.GetHumidityFeedData().Result);
        }

        [HttpGet("GetPhotoResistorFeedData", Name = "GetPhotoResistorFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetPhotoResistorFeedData()
        {
            return Ok(_adafruitService.GetPhotoResistorFeedData().Result);
        }

        [HttpGet("GetPlantHeightData", Name = "GetPlantHeightData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetPlantHeightData()
        {
            return Ok(_adafruitService.GetPlantHeightData().Result);
        }

        [HttpGet("GetPumpTriggeredFeedData", Name = "GetPumpTriggeredFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetPumpTriggeredFeedData()
        {
            return Ok(_adafruitService.GetPumpTriggeredFeedData().Result);
        }

        [HttpGet("GetSoilMoistureFeedData", Name = "GetSoilMoistureFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetSoilMoistureFeedData()
        {
            return Ok(_adafruitService.GetSoilMoistureFeedData().Result);
        }

        [HttpGet("GetTemperatureFeedData", Name = "GetTemperatureFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetTemperatureFeedData()
        {
            return Ok(_adafruitService.GetTemperatureFeedData().Result);
        }

        [HttpGet("GetWaterLevelFeedData", Name = "GetWaterLevelFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedModel<float>>> GetWaterLevelFeedData()
        {
            return Ok(_adafruitService.GetWaterLevelFeedData().Result);
        }
    }
}
