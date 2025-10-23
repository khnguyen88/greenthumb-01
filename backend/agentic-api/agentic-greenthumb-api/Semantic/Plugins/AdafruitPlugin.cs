using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using System.ComponentModel;
using System.Text.Json;


namespace AgenticGreenthumbApi.Semantic.Plugins
{
    public class AdafruitPlugin
    {
        private readonly AdafruitService _adafruitService;

        public AdafruitPlugin(AdafruitService adafruitService)
        {
            _adafruitService = adafruitService;
        }

        public static class KernelFunction
        {
            public const string GetAdafruitGrowLightFeedData = nameof(GetAdafruitGrowLightFeedData);
            public const string GetAdafruitHumidityFeedData = nameof(GetAdafruitHumidityFeedData);
            public const string GetAdafruitPhotoResistorFeedData = nameof(GetAdafruitPhotoResistorFeedData);
            public const string GetAdafruitPlantHeightFeedData = nameof(GetAdafruitPlantHeightFeedData);
            public const string GetAdafruitPumpTriggeredFeedData = nameof(GetAdafruitPumpTriggeredFeedData);
            public const string GetAdafruitSoilMoistureFeedData = nameof(GetAdafruitSoilMoistureFeedData);
            public const string GetAdafruitTemperatureFeedData = nameof(GetAdafruitTemperatureFeedData);
            public const string GetAdafruitWaterLevelFeedData = nameof(GetAdafruitWaterLevelFeedData);

            public const string FilterAdafruitSensorFeedData = nameof(FilterAdafruitSensorFeedData);
            public const string FormatAdafruitSensorFeedData = nameof(FormatAdafruitSensorFeedData);
            public const string SummarizeAdafruitSensorFeedData = nameof(SummarizeAdafruitSensorFeedData);
            public const string AnalyzeAdafruitSensorFeedData = nameof(AnalyzeAdafruitSensorFeedData);
        }

        [KernelFunction(KernelFunction.GetAdafruitGrowLightFeedData)]
        [Description("Retrieves the IoT Automated Gardening System device's grow light feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System device's grow light feed data from Adafruit IO Server.")]
        public string GetAdafruitGrowLightFeedData()
        {
            var taskData = _adafruitService.GetGrowLightFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitHumidityFeedData)]
        [Description("Retrieves the IoT Automated Gardening System DHT11 sensor's humidity feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System DHT11 sensor's humidity feed data from Adafruit IO Server.")]
        public string GetAdafruitHumidityFeedData()
        {
            var taskData = _adafruitService.GetHumidityFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitPhotoResistorFeedData)]
        [Description("Retrieves the IoT Automated Gardening System sensor's photoresistor feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System sensor's photoresistor feed data from Adafruit IO Server.")]
        public string GetAdafruitPhotoResistorFeedData()
        {
            var taskData = _adafruitService.GetPhotoResistorFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitPlantHeightFeedData)]
        [Description("Retrieves the IoT Automated Gardening System sonar distance sensor's plant height feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System sonar distance sensor's plant height feed data from Adafruit IO Server.")]
        public string GetAdafruitPlantHeightFeedData()
        {
            var taskData = _adafruitService.GetPlantHeightData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitPumpTriggeredFeedData)]
        [Description("Retrieves the IoT Automated Gardening System device's pump triggered feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System device's pump triggered feed data from Adafruit IO Server.")]
        public string GetAdafruitPumpTriggeredFeedData()
        {
            var taskData = _adafruitService.GetPumpTriggeredFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitSoilMoistureFeedData)]
        [Description("Retrieves the IoT Automated Gardening System soil conductivity sensor's soil moisture feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System soil conductivity sensor's soil moisture feed data from Adafruit IO Server.")]
        public string GetAdafruitSoilMoistureFeedData()
        {
            var taskData = _adafruitService.GetSoilMoistureFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitTemperatureFeedData)]
        [Description("Retrieves the IoT Automated Gardening System DHT11 sensor's temperature feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System DHT11 sensor's temperature feed data from Adafruit IO Server.")]
        public string GetAdafruitTemperatureFeedData()
        {
            var taskData = _adafruitService.GetTemperatureFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.GetAdafruitWaterLevelFeedData)]
        [Description("Retrieves the IoT Automated Gardening System sonar distance sensor's water level feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System sonar distance sensor's water level feed data from Adafruit IO Server.")]
        public string GetAdafruitWaterLevelFeedData()
        {
            var taskData = _adafruitService.GetWaterLevelFeedData();
            IEnumerable<AdafruitFeedDto<float>> data = taskData.Result ?? new List<AdafruitFeedDto<float>>();
            return JsonSerializer.Serialize(data);
        }

        [KernelFunction(KernelFunction.FilterAdafruitSensorFeedData)]
        [Description("Filter the feed data as requested upon recieving it. Only if requested. For example if user wants to filter sensor data at an interval of every other hour. Filters should fit the properties of the feed data model.")]
        [return: Description("Filter the the model data as requested upon recieving it. For example if user wants to filter sensor data at an interval of every other hour. If the requested filter is not achievable return the content and apologize.")]
        public string FilterAdafruitSensorFeedData(IEnumerable<AdafruitFeedModel<float>> unfilteredData, string filterRequest)
        {
            return $"Filter the feed data as requested upon recieving it. Only if requested. For example if the user wants you to find the median temperature of humidity of each day or of the duration when the program was running do it. \n Given this data: {JsonSerializer.Serialize(unfilteredData)} and filter request type if any: {filterRequest}. If there is no specifi request type come up with your own. Account for the context.";
        }

        [KernelFunction(KernelFunction.FormatAdafruitSensorFeedData)]
        [Description("Format the feed data as requested upon recieving it. Only if requested. For example if user wants to format sensor data into an ascii table format with proper spacing with specific properties, you should do that. Format should fit the properties of the feed data model.")]
        [return: Description("Format the the model data as requested upon recieving it.  For example if user wants to format sensor data into an ascii table format with proper spacing with specific properties, you should do that If the requested format is not achievable return the content and apologize.")]
        public string FormatAdafruitSensorFeedData(IEnumerable<AdafruitFeedModel<float>> unfilteredData, string formatRequest)
        {
            return $"Format the feed data as requested upon recieving it. Only if requested. For example if the user wants you to find the median temperature of humidity of each day or of the duration when the program was running do it. \n Given this data: {JsonSerializer.Serialize(unfilteredData)} and format request type if any: {formatRequest}. If there is no specifi request type come up with your own. Account for the context.";
        }

        [KernelFunction(KernelFunction.SummarizeAdafruitSensorFeedData)]
        [Description("Summarize the feed data as requested upon recieving it. Only if requested. For example if the user wants you to find the median temperature of humidity of each day or of the duration when the program was running do it.")]
        [return: Description("Return a summary of the feed data as requested.")]
        public string SummarizeAdafruitSensorFeedData(IEnumerable<AdafruitFeedModel<float>> unfilteredData, string summaryRequest)
        {
            return $"Summarize the feed data as requested upon recieving it. Only if requested. For example if the user wants you to find the median temperature of humidity of each day or of the duration when the program was running do it. \n Given this data: {JsonSerializer.Serialize(unfilteredData)} and summary request type if any: {summaryRequest}. If there is no specifi request type come up with your own. Account for the context.";
        }

        [KernelFunction(KernelFunction.AnalyzeAdafruitSensorFeedData)]
        [Description("Analyze the feed data as requested upon recieving it. Only if requested. Find any interesting patterns and identifies if could has any outlier and issue and determine if its possible sensor issues.")]
        [return: Description("Return an analysis report of the feed data as requested.")]
        public string AnalyzeAdafruitSensorFeedData(IEnumerable<AdafruitFeedModel<float>> unfilteredData, string analysisRequest)
        {
            return $"Analyze the feed data as requested upon recieving it. Only if requested. Find any interesting patterns and identifies if could has any outlier and issue and determine if its possible sensor issues. \n Given this data: {JsonSerializer.Serialize(unfilteredData)} and analysis request type if any: {analysisRequest}. If there is no specified request type come up with your own. Account for the context.";
        }
    }
}