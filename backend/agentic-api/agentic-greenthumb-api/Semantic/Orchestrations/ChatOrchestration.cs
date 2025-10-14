using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Agents.Magentic;

using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;

#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatOrchestration
    {
        public MagenticOrchestration MagenticOrchestration { get; set; }
        public ChatOrchestration(ProjectInfoAgentRegistry chatAgentRegistry, AdafruitFeedAgentRegistry adafruitFeedAgentRegistry)
        {
            //Kernel
            Kernel managerKernel = KernelFactoryHelper.GetNewKernel();

            //Chat History
            ChatHistory history = [];

            ValueTask ResponseCallback(ChatMessageContent response)
            {
                Console.WriteLine();
                Console.WriteLine($"# {response.Role} - {response.AuthorName}: {response.Content}");
                Console.WriteLine();
                history.Add(response);


                return ValueTask.CompletedTask;
            }

            //Manager
            StandardMagenticManager manager = new StandardMagenticManager(
                managerKernel.GetRequiredService<IChatCompletionService>(),
                new OpenAIPromptExecutionSettings())
            {
                MaximumInvocationCount = 5
            };

            //Agents
            var projectInfo = chatAgentRegistry.ProjectInfoAgent;
            var adafruitFeedAgent = adafruitFeedAgentRegistry.AdafruitFeedAgent;

            //Orchestration
            // =====================================================================================
            MagenticOrchestration = new MagenticOrchestration(
                manager,
                projectInfo,
                adafruitFeedAgent
            )
            {
                ResponseCallback = ResponseCallback,
            };
        }
    }
}
#pragma warning enable
