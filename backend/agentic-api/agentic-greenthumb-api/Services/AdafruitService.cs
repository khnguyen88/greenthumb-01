using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Models;

namespace AgenticGreenthumbApi.Services
{
    public class AdafruitService
    {
        private readonly AdafruitAPIClient _adafruitAPIClient;
        public AdafruitService(AdafruitAPIClient adafruitAPIClient) { 
            _adafruitAPIClient = adafruitAPIClient;
        }

        public async Task<IEnumerable<AdafruitFeedModel<bool>>> GetGrowLightFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetGrowLightFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetHumidityFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetHumidityFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetPhotoResistorFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetPhotoResistorFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetPlantHeightData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetPlantHeightData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<bool>>> GetPumpTriggeredFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetPumpTriggeredFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetSoilMoistureFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetSoilMoistureFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetTemperatureFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetTemperatureFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetWaterLevelFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            return await _adafruitAPIClient.GetWaterLevelFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
        }
    }
}
