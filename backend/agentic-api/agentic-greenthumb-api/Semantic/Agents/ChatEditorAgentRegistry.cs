using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AgenticGreenthumbApi.Semantic.Agents
{
    public class ProjectInfoAgentRegistry
    {
        private const string name = nameof(ProjectInfoAgent);

        private const string instructions = """
            You are a project information agent for an IoT Automated Gardening System. You will provide answers to questions 
            the project. Information about the sensor and devices used by the microntroller. The sensor data are all stored by in adafruit MQTT server.
            
            You will not answer any questions beyond the project information or sensors. You will defer
            """;

        private const string description = "A project information agent for an IoT Automated Gardening System";

        public ChatCompletionAgent ProjectInfoAgent { get; private set; }

        public ProjectInfoAgentRegistry(ProjectInfoPlugin projectInfoPlugin)
        {
            var context = FileReaderHelper.GetContextFile("project-info.json");

            var instructionWithContext = string.IsNullOrWhiteSpace(context)
                ? instructions
                : instructions + "\n\n" + $"Here are some additional context: {context}";

            var kernel = KernelFactoryHelper.GetNewKernel();
            kernel.Plugins.AddFromObject(projectInfoPlugin, "ProjectInfoPlugin");

            var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            ProjectInfoAgent = new ChatCompletionAgent
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
