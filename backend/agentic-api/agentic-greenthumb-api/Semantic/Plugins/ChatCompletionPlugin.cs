using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using System.ComponentModel;
using System.Text.Json;

namespace AgenticGreenthumbApi.Semantic.Plugins
{
    public class ChatCompletionPlugin
    {
        private readonly AdafruitPlugin _adafruitPlugin;
        private readonly AdafruitService _adafruitService;

        public static class KernelFunction
        {
            public const string GetAdafruitSensorFeedDataAsync = nameof(GetAdafruitSensorFeedDataAsync);
            public const string GetHumidityData = nameof(GetHumidityData);
        }

        //NOTE: PROCESS AND ORCESTRATION ALLOWS AGENTS TO COMMUNICATE AND RELAY TO EACH OTHER. WE CANNOT CALL AN AGENT INSIDE A PLUGIN ASSIGNED TO ANOTHER AGENT.

        public ChatCompletionPlugin(AdafruitPlugin adafruitPlugin, AdafruitService adafruitService)
        {
            _adafruitPlugin = adafruitPlugin;
            _adafruitService = adafruitService;
        }

        [KernelFunction(KernelFunction.GetHumidityData)]
        [Description("Filter the feed data as requested upon recieving it. Only if requested. For example if user what to filter sensor data that was read every other ours. Filters should fit the properties of the feed data model.")]
        [return: Description("Filtered the model data as requested upon recieving it. For example if user what to filter sensor data that was read every other ours. If the requested filter is not achievable return the content and apologize.")]
        public async Task<string> GetHumidityData(IEnumerable<AdafruitFeedModel<float>> unfilteredData)
        {
            return JsonSerializer.Serialize(Task.FromResult(_adafruitService.GetHumidityFeedData()));
        }

        //[KernelFunction(KernelFunction.GetAdafruitSensorFeedDataAsync)]
        //[Description("Retrieves all of the requested data")]
        //[return: Description("Returns the sensor feed data retrieved from adafruit IO server in a nice ascii text format. Applies any filter and provides any analysis and summary for the specified data as requested by the user.")]
        //public async Task<string> GetAdafruitSensorFeedDataAsync(List<string> sensorFeedName)
        //{

        //    AdafruitFeedAgentRegistry adafruitFeedAgentRegistry = new AdafruitFeedAgentRegistry(_adafruitPlugin);

        //    ChatHistoryAgentThread agentThread = new();

        //    string prompt = $"Obtain me all of the sensor data requested by the users from the Adafruit IO Server. Format it in a readable table format. Analyze, summarize, and filter the data if the user requests it. These are the requested sensor feed the user wanted: \n {JsonSerializer.Serialize(sensorFeedName)}";

        //    ChatMessageContent message = await adafruitFeedAgentRegistry.AdafruitFeedAgent.InvokeAsync(prompt, agentThread).FirstAsync();

        //    Console.WriteLine(message.ToString());

        //    await agentThread.DeleteAsync();

        //    return message.Content;
        //}
    }
}
