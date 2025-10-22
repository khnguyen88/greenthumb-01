using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Mappers;
using AgenticGreenthumbApi.Models;
using NRedisStack.Search;
using System.Text.Json;
using static Microsoft.VisualStudio.Threading.AsyncReaderWriterLock;

namespace AgenticGreenthumbApi.Services
{
    public class AdafruitService
    {
        private readonly AdafruitAPIClient _adafruitAPIClient;
        private string _configPath;
        private readonly IConfigurationRoot _config;
        private readonly string contextFile = "adafruit-feed-info.json";
        public AdafruitService(AdafruitAPIClient adafruitAPIClient) { 
            _adafruitAPIClient = adafruitAPIClient;


            _configPath = Path.Combine(Environment.CurrentDirectory, "Semantic", "Contexts", contextFile);
            Console.WriteLine(File.Exists(_configPath));
            _config = new ConfigurationBuilder().AddJsonFile(_configPath).Build();


        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetGrowLightFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            //NOTE: MAP REQUITED, BECAUSE THIS DATA WILL BE INGESTED BY LLM AND SENT WITH PROMPT. WE NEED TO REDUCE THE AMOUNT OF UNCESSARY INFORMATION PROVIDED IN THE PROMPT AND REDUCE OUR TOKEN, TO ALLOW LLM TO PROCESS THINGS QUICKER.

            var data =  await _adafruitAPIClient.GetGrowLightFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("GrowlightTriggered");
            Console.WriteLine(dataUnitType);
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetHumidityFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetHumidityFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("Humidity");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetPhotoResistorFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetPhotoResistorFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("PhotoResistor");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetPlantHeightData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetPlantHeightData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("PlantHeight");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetPumpTriggeredFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetPumpTriggeredFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("PumpTriggered");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetSoilMoistureFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetSoilMoistureFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("SoilMoisture");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetTemperatureFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetTemperatureFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("Temperature");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeed<float>>> GetWaterLevelFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetWaterLevelFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var mappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("WaterLevel");
            mappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(mappedData);

            return mappedData;
        }

        private string GetFeedDataUnit(string feedNameFromContext)
        {
            string configPath = $"AdafruitIO:FeedNameUnits:{feedNameFromContext}:AbbreviatedUnit";
            return _config[configPath] ?? string.Empty;
        }

        private void SortFeedDataByAscending<T>(List<AdafruitFeed<T>> dataList)
        {
            dataList.Sort((x, y) => x.CreatedAt.CompareTo(y.CreatedAt));
        }
    }
}
