using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;

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

        public ChatCompletionAgent ChatCompletionAgent { get; private set; }

        public ChatAgentRegistry(ChatCompletionPlugin chatCompletionPlugin)
        {
            var context = FileReaderHelper.GetContextFile("project-info.json");
            var instructionWithContext = string.IsNullOrWhiteSpace(context)
                ? instructions
                : instructions + "\n\n" + $"Here are some additional context: {context}";

            var kernel = KernelFactoryHelper.GetNewKernel();
            kernel.Plugins.AddFromObject(chatCompletionPlugin, "ChatCompletionPlugin");

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            ChatCompletionAgent = new ChatCompletionAgent
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
