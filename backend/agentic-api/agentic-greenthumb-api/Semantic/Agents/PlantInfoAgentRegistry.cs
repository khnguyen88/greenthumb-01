using AgenticGreenthumbApi.Helper;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AgenticGreenthumbApi.Semantic.Agents
{
    public class PlantInfoAgentRegistry
    {
        private const string name = nameof(PlantInfoAgent);

        private const string instructions = """
        You are a plant information specialist and will provide plant related information to help the user configure some parameters for their microcontroller.

        Summmarize and a short description of the plants needs and other facts about it.

        But also format it to contain all of the properties specified in the PlantInfo domain file and it's associated properties value based on the description of the plant.
        """;

        private const string description = "A plant information broker agent for an IoT Automated Gardening System";

        public ChatCompletionAgent PlantInfoAgent { get; private set; }

        public PlantInfoAgentRegistry()
        {
            var contextDomainFile = FileReaderHelper.GetDomainFile("PlantInfo.cs");

            var contextDomainPropertyValue = FileReaderHelper.GetDomainFile("PlantPropertyValue.cs");

            var instructionWithContext = string.IsNullOrWhiteSpace(contextDomainFile) && string.IsNullOrWhiteSpace(contextDomainPropertyValue)
                ? instructions
                : instructions + "\n\n" + $"Here are some additional context about how the data should be format with the following properties and schema. Make sure its as JSON format with actual values: {contextDomainFile}"
                               + "\n\n" + $"And based on the description the plants needs and attributes, they should mapped against these enums to meet the specified values: {contextDomainPropertyValue}";


            var kernel = KernelFactoryHelper.GetNewKernel();

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            PlantInfoAgent = new ChatCompletionAgent
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
