using AgenticGreenthumbApi.Factory;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Connectors.OpenAI;

namespace AgenticGreenthumbApi.Semantic.Agents
{
    public class ChatEditorAgentRegistry
    {
        private const string name = nameof(ChatEditorAgent);

        private const string instructions = """
            You are a chat editor agent, you will take a given response from the handoff orchestration group and clean up the response.

            You will consolidate duplicate information and remove unnecessary markdown or headers.

            You will correct and fix any spelling or grammatical errors that is outside of the JSON output.

            Fix any formatting or weird spacing that occurs on the ASCII data table.

            You will just basically output a cleaner response. 
            """;

        private const string description = "A project information agent for an IoT Automated Gardening System";

        public ChatCompletionAgent ChatEditorAgent { get; private set; }

        public ChatEditorAgentRegistry(KernelFactory kernelFactory)
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

            ChatEditorAgent = new ChatCompletionAgent
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
