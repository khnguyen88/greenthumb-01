using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AgenticGreenthumbApi.Agents
{
    public class DataAnalystAgent
    {
        private const string name = nameof(AdafruitFeedAgent);

        private const string instructions = """
            You are an Adafruit IO feed data agent for an IoT Automated Gardening System. The gardening system contains a microntroller that sensor and device data to the Adafruit IO server. 
            
            You will retrieve any sensor and device data requested by the user. If the user asks for all data you will retrieve all of the data. If the user asks you to provide feed data a specific feed or sensor you will provide that.
            
            If the user requests that you filter the data or simplify it. You will do it as request.

            You may not change or manipulate the original values or the properties int he value. However you can filter data or summarize it, for example, if the user request that he wants the average value for a week, 
            you can calculate and provide that. If the users want data in intervals of every other hours or every six hours you can provide that too.

            If there is no existing feed for a specific parameter that the user asks, then specify that you cannot find such.
            """;

        private const string description = """
            An Adafruit IO feed data agent for an IoT Automated Gardening System. The gardening system contains a microntroller that sensor and device data to the Adafruit IO server. 
            The agent will retrieve any sensor and device data requested by the user. The agent my summarize the data too
            """;

        public ChatCompletionAgent AdafruitFeedAgent { get; private set; }

        public AdafruitFeedAgentRegistry(AdafruitPlugin adafruitPlugin)
        {
            var context = FileReaderHelper.GetContextFile("adafruit-feed-info.json");
            var instructionWithContext = string.IsNullOrWhiteSpace(context)
                ? instructions
                : instructions + "\n\n" + $"Here are some additional context: {context}";

            var kernel = KernelFactoryHelper.GetNewKernel();
            kernel.Plugins.AddFromObject(adafruitPlugin, "AdafruitPlugin");

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            AdafruitFeedAgent = new ChatCompletionAgent
            {
                Name = name,
                Instructions = instructionWithContext,
                Description = description,
                Kernel = kernel,
                Arguments = new KernelArguments(openAIPromptExecutionSettings),
            };
        }
    }
}
