using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Dtos;
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
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetGrowLightFeedData()
        {
            return Ok(_adafruitService.GetGrowLightFeedData().Result);
        }

        [HttpGet("GetHumidityFeedData", Name = "GetHumidityFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetHumidityFeedData()
        {
            return Ok(_adafruitService.GetHumidityFeedData().Result);
        }

        [HttpGet("GetPhotoResistorFeedData", Name = "GetPhotoResistorFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetPhotoResistorFeedData()
        {
            return Ok(_adafruitService.GetPhotoResistorFeedData().Result);
        }

        [HttpGet("GetPlantHeightData", Name = "GetPlantHeightData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetPlantHeightData()
        {
            return Ok(_adafruitService.GetPlantHeightData().Result);
        }

        [HttpGet("GetPumpTriggeredFeedData", Name = "GetPumpTriggeredFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetPumpTriggeredFeedData()
        {
            return Ok(_adafruitService.GetPumpTriggeredFeedData().Result);
        }

        [HttpGet("GetSoilMoistureFeedData", Name = "GetSoilMoistureFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetSoilMoistureFeedData()
        {
            return Ok(_adafruitService.GetSoilMoistureFeedData().Result);
        }

        [HttpGet("GetTemperatureFeedData", Name = "GetTemperatureFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetTemperatureFeedData()
        {
            return Ok(_adafruitService.GetTemperatureFeedData().Result);
        }

        [HttpGet("GetWaterLevelFeedData", Name = "GetWaterLevelFeedData")]
        public ActionResult<IEnumerable<AdafruitFeedDto<float>>> GetWaterLevelFeedData()
        {
            return Ok(_adafruitService.GetWaterLevelFeedData().Result);
        }
    }
}
