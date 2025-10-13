using AgenticGreenthumbApi.Controllers;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Models;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ML.OnnxRuntimeGenAI;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AgenticGreenthumbApi.Client
{
    public class AdafruitAPIClient
    {
        private readonly ILogger<AdafruitAPIClient> _logger;
        private static HttpClient _client = new();
        private static readonly IConfigurationRoot _config = new ConfigurationBuilder()
            .AddUserSecrets("4f91f0a7-edfa-4d74-b7d8-6f7a324e86fb")
            .Build();

        public AdafruitAPIClient(ILogger<AdafruitAPIClient> logger) { 
            _logger = logger;
        }

        private async Task<IEnumerable<AdafruitFeedModel<T>>> GetFeedData<T>(string feedName, string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            string aIOBaseUri = _config["AdafruitIO:BaseUri"]!;
            string aIOKey = _config["AdafruitIO:APIKey"]!;
            string aIOUsername = _config["AdafruitIO:Username"]!;
            string feedKey = feedKeyPrefixMacAddress.IsNullOrEmpty() || feedKeyPrefixStartDate.IsNullOrEmpty() ? feedName : $"{feedKeyPrefixMacAddress}-{feedKeyPrefixStartDate}-{feedName}"; 

            var url = $"{aIOBaseUri}/{aIOUsername}/feeds/{feedKey}/data";
            Console.WriteLine($"Requesting URL: {url}");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("X-AIO-Key", aIOKey);

            try
            {
                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string feedString = await response.Content.ReadAsStringAsync();

                List<AdafruitFeedModel<string>> rawFeedModel = string.IsNullOrWhiteSpace(feedString) ? new List<AdafruitFeedModel<string>>() : JsonSerializer.Deserialize<List<AdafruitFeedModel<string>>>(feedString) ?? new List<AdafruitFeedModel<string>>();

                List<AdafruitFeedModel<T>> convertedFeedModel =
                    rawFeedModel.Select(x => new AdafruitFeedModel<T>
                    {
                        Id = x.Id,
                        FeedId = x.FeedId,
                        FeedKey = x.FeedKey,
                        CreatedAt = x.CreatedAt,
                        CreatedEpoch = x.CreatedEpoch,
                        Expiration = x.Expiration,
                        Value = TypeCastHelper.ChangeType<T>(x.Value)
                    }).ToList();
                
                return convertedFeedModel;
            }
            catch (Exception ex)
            {

                _logger.LogError(message: ex.Message, ex);

                return new List<AdafruitFeedModel<T>>() { };
            }
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetGrowLightFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string growLightFeedName = _config["AdafruitIO:FeedNames:GrowlightTriggered"]!;

            return await GetFeedData<float>(growLightFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetHumidityFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string humidityFeedName = _config["AdafruitIO:FeedNames:Humidity"]!;

            return await GetFeedData<float>(humidityFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetPhotoResistorFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string photoResistorFeedName = _config["AdafruitIO:FeedNames:PhotoResistor"]!;

            return await GetFeedData<float>(photoResistorFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetPlantHeightData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string plantHeightFeedName = _config["AdafruitIO:FeedNames:PlantHeight"]!;

            return await GetFeedData<float>(plantHeightFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetPumpTriggeredFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string pumpTriggeredFeedName = _config["AdafruitIO:FeedNames:PumpTriggered"]!;

            return await GetFeedData<float>(pumpTriggeredFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetSoilMoistureFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string soilMoistureFeedName = _config["AdafruitIO:FeedNames:SoilMoisture"]!;

            return await GetFeedData<float>(soilMoistureFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetTemperatureFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string temperatureFeedName = _config["AdafruitIO:FeedNames:Temperature"]!;

            return await GetFeedData<float>(temperatureFeedName);
        }

        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetWaterLevelFeedData(string feedKeyPrefixMacAddress, string feedKeyPrefixStartDate)
        {
            string waterLevelFeedName = _config["AdafruitIO:FeedNames:GrowlightTriggered"]!;

            return await GetFeedData<float>(waterLevelFeedName);
        }
    }
}