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

        AdafruitPlugin(AdafruitService adafruitService)
        {
            _adafruitService = adafruitService;
        }
        public static class KernelFunction
        {
            public const string GetAdafruitSensorFeedDataAsync = nameof(GetAdafruitSensorFeedDataAsync);
            public const string FormatAdafruitSensorFeedDataAsync = nameof(FormatAdafruitSensorFeedDataAsync);
            public const string SummarizeAdafruitSensorFeedDataAsync = nameof(SummarizeAdafruitSensorFeedDataAsync);
            public const string AnalyzeAdafruitSensorFeedDataAsync = nameof(AnalyzeAdafruitSensorFeedDataAsync);
        }

        [KernelFunction(KernelFunction.GetAdafruitSensorFeedDataAsync)]
        [Description("Retrieves the IoT Automated Gardening System sensor's humidity feed data from Adafruit IO Server through a webclient")]
        [return: Description("The IoT Automated Gardening System sensor's humidity feed data from Adafruit IO Server.")]
        public async Task<IEnumerable<AdafruitFeedModel<float>>> GetAdafruitHumidityFeedData()
        {
            return await _adafruitService.GetHumidityFeedData();
        }

        [KernelFunction(KernelFunction.FormatAdafruitSensorFeedDataAsync)]
        [Description("Filter the feed data as requested upon recieving it. Only if requested. For example if user what to filter sensor data that was read every other ours. Filters should fit the properties of the feed data model.")]
        [return: Description("Filtered the model data as requested upon recieving it. For example if user what to filter sensor data that was read every other ours. If the requested filter is not achievable return the content and apologize.")]
        public async Task<IEnumerable<AdafruitFeedModel<float>>> FormatAdafruitSensorFeedDataAsync(IEnumerable<AdafruitFeedModel<float>> unfilteredData)
        {
            return await _adafruitService.GetHumidityFeedData();
        }

        [KernelFunction(KernelFunction.SummarizeAdafruitSensorFeedDataAsync)]
        [Description("Summarize the feed data as requested upon recieving it. Only if requested. For example if the user wants you to find the median temperature of humidity of each day or of the duration when the program was running do it.")]
        [return: Description("Return a summary of the feed data as requested.")]
        public async Task<string> SummarizeAdafruitSensorFeedDataAsync(IEnumerable<AdafruitFeedModel<float>> unfilteredData, string summaryRequest)
        {
            return $"Summarize the feed data as requested upon recieving it. Only if requested. For example if the user wants you to find the median temperature of humidity of each day or of the duration when the program was running do it. \n Given this data: {JsonSerializer.Serialize(unfilteredData)} and summary request type if any: {summaryRequest}. If there is no specifi request type come up with your own";
        }

        [KernelFunction(KernelFunction.AnalyzeAdafruitSensorFeedDataAsync)]
        [Description("Analyze the feed data as requested upon recieving it. Only if requested. Find any interesting patterns and identifies if could has any outlier and issue and determine if its possible sensor issues.")]
        [return: Description("Return an analysis report of the feed data as requested.")]
        public async Task<string> AnalyzeAdafruitSensorFeedDataAsync(IEnumerable<AdafruitFeedModel<float>> unfilteredData, string analysisRequest)
        {
            return $"Analyze the feed data as requested upon recieving it. Only if requested. Find any interesting patterns and identifies if could has any outlier and issue and determine if its possible sensor issues. \n Given this data: {JsonSerializer.Serialize(unfilteredData)} and analysis request type if any: {analysisRequest}. If there is no specified request type come up with your own.";
        }


    }
}
