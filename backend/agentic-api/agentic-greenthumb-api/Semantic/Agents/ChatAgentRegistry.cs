using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Plugins;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;

namespace AgenticGreenthumbApi.Semantic.Agents
{
    public class ChatAgentRegistry
    {
        private const string name = nameof(ChatCompletionAgent);

        private const string instructions = """
            You are a chat completion agent for an IoT Automated Gardening System. You will communicate with other agents through plugins to answer questions 
            about pattern from sensor data of the gardening system. The sensor data are all stored by in adafruit MQTT server.
            
            You will also provide information about plants to help user make decisions parameters they need to adjust in the microcontroller. 
            However you should not answer any questions that is outside of sensor data or plant gardening information.
            """;

        private const string description = "A chat completion agent for an IoT Automated Gardening System";

        private static string context;

        private static string instructionWithContext;

        private static Kernel kernel;

        static ChatAgentRegistry()
        {
            context = FileReaderHelper.GetContextFile("project-info.json");
            instructionWithContext = string.IsNullOrWhiteSpace(context) ? instructions : instructions + "\n \n" + $"Here are some additional context: {context}";
            kernel = KernelFactoryHelper.GetNewKernel();
            kernel.Plugins.AddFromType<ChatCompletionPlugin>("ChatCompletionPlugin");
        }

        private static readonly OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
        {
            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
        };

        public ChatCompletionAgent ChatCompletionAgent {  get ; } = new()
        {
            Name = name,
            Instructions = instructionWithContext,
            Description = description,
            Kernel = kernel,
            Arguments = new KernelArguments(openAIPromptExecutionSettings),
        };

    }
}
