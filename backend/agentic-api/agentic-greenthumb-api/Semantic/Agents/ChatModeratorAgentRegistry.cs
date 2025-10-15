using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;


namespace AgenticGreenthumbApi.Semantic.Agents
{
    public class ChatModeratorAgentRegistry
    {
        private const string name = nameof(ChatModeratorAgent);

        private const string instructions = """
            You are a chat completion moderator agen for an IoT Automated Gardening System. 
            
            You will make sure the user prompt conversation does not deviate from the the topics of the IoT Automated Gardening System, Plant Information, Adafruit MQTT System, Adafruit Feed Data.
            
            You will not answer any questions beyond those topics, and apologize if you can not answer them or handoff the prompt to another agents who could answer them. This is done through the handoff orchestration.

            For example if the user prompt you to ask about computer 2 numbers, the city of a state, or information about a random animal, you will apologize and say you cannot answer that.

            If the user has questions about the project, then handoff the prompt to the ProjectInfoAgent. 
            
            If the user has a question about sensor's feed data stored in Adafruit IO and to format, filter, analyze, or summarize in then handoff the promot the Adafruit Feed Agent.

            If the use rhas questions about plant information then you will handoff the promot to the PlantInfoAgent.
            """;

        private const string description = "A chat completion moderator agent for an IoT Automated Gardening System";

        public ChatCompletionAgent ChatModeratorAgent { get; private set; }

        public ChatModeratorAgentRegistry()
        {
            var context = "";

            var instructionWithContext = string.IsNullOrWhiteSpace(context)
                ? instructions
                : instructions + "\n\n" + $"Here are some additional context: {context}";

            var kernel = KernelFactoryHelper.GetNewKernel();

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            ChatModeratorAgent = new ChatCompletionAgent
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
