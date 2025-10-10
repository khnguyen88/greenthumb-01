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
        public static class KernelFunction
        {
            public const string GetAdafruitSensorFeedDataAsync = nameof(GetAdafruitSensorFeedDataAsync);
        }

        [KernelFunction(KernelFunction.GetAdafruitSensorFeedDataAsync)]
        [Description("Retrieves all of the requested data")]
        [return: Description("Returns the sensor feed data retrieved from adafruit IO server in a nice ascii text format. Applies any filter and provides any analysis and summary for the specified data as requested by the user.")]
        public async Task<string> GetAdafruitSensorFeedDataAsync(List<string> sensorFeedName)
        {

            AdafruitFeedAgentRegistry adafruitFeedAgentRegistry = new AdafruitFeedAgentRegistry();

            ChatHistoryAgentThread agentThread = new();

            string prompt = $"Obtain me all of the sensor data requested by the users from the Adafruit IO Server. Format it in a readable table format. Analyze, summarize, and filter the data if the user requests it. These are the requested sensor feed the user wanted: \n {JsonSerializer.Serialize(sensorFeedName)}";

            ChatMessageContent message = await adafruitFeedAgentRegistry.AdafruitFeedAgent.InvokeAsync(prompt, agentThread).FirstAsync();

            Console.WriteLine(message.ToString());

            await agentThread.DeleteAsync();

            return message.Content;
        }
    }
}
