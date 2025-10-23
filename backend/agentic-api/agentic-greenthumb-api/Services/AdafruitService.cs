using AgenticGreenthumbApi.Client;
using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
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

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetGrowLightFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            //NOTE: MAP REQUITED, BECAUSE THIS DATA WILL BE INGESTED BY LLM AND SENT WITH PROMPT. WE NEED TO REDUCE THE AMOUNT OF UNCESSARY INFORMATION PROVIDED IN THE PROMPT AND REDUCE OUR TOKEN, TO ALLOW LLM TO PROCESS THINGS QUICKER.

            var data =  await _adafruitAPIClient.GetGrowLightFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("GrowlightTriggered");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetHumidityFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetHumidityFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("Humidity");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetPhotoResistorFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetPhotoResistorFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("PhotoResistor");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetPlantHeightData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetPlantHeightData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("PlantHeight");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetPumpTriggeredFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetPumpTriggeredFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("PumpTriggered");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetSoilMoistureFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetSoilMoistureFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("SoilMoisture");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetTemperatureFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetTemperatureFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();

            string dataUnitType = GetFeedDataUnit("Temperature");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

            return mappedData;
        }

        public async Task<IEnumerable<AdafruitFeedDto<float>>> GetWaterLevelFeedData(string feedKeyPrefixMacAddress = "", string feedKeyPrefixStartDate = "")
        {
            var data = await _adafruitAPIClient.GetWaterLevelFeedData(feedKeyPrefixMacAddress, feedKeyPrefixStartDate);
            var partialMappedData = data.Select(m => AdafruitFeedMapper<float>.ModelToDomainMapper(m)).ToList();
    
            string dataUnitType = GetFeedDataUnit("WaterLevel");
            partialMappedData.ForEach(item => item.Unit = dataUnitType);
            SortFeedDataByAscending<float>(partialMappedData);

            var mappedData = partialMappedData.Select(d => AdafruitFeedMapper<float>.DomainToDtoMapper(d)).ToList();

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
