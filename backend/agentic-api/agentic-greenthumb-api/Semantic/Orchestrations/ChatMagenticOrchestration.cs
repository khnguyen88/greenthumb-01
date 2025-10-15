using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatMagenticOrchestration
    {
        public MagenticOrchestration MagenticOrchestration { get; set; }
        public ChatHistory ChatHistory {get; set;} = [];

        public ChatMagenticOrchestration(ChatModeratorAgentRegistry chatModeratorAgentRegistry, ProjectInfoAgentRegistry projectInfoAgentRegistry, AdafruitFeedAgentRegistry adafruitFeedAgentRegistry, PlantInfoAgentRegistry plantInfoAgentRegistry)
        {
            //Kernel
            Kernel managerKernel = KernelFactoryHelper.GetNewKernel();

            //Chat History
            ChatHistory = [];

            ValueTask ResponseCallback(ChatMessageContent response)
            {
                Console.WriteLine();
                Console.WriteLine($"# {response.Role} - {response.AuthorName}: {response.Content}");
                Console.WriteLine();
                ChatHistory.Add(response);


                return ValueTask.CompletedTask;
            }

            //Manager
            StandardMagenticManager manager = new StandardMagenticManager(
                managerKernel.GetRequiredService<IChatCompletionService>(),
                new OpenAIPromptExecutionSettings())
            {
                MaximumInvocationCount = 2, //Very important settings
            };

            //Agents
            var chatModeratorAgent = chatModeratorAgentRegistry.ChatModeratorAgent;
            var projectInfoAgent = projectInfoAgentRegistry.ProjectInfoAgent;
            var adafruitFeedAgent = adafruitFeedAgentRegistry.AdafruitFeedAgent;
            var plantInfoAgent = plantInfoAgentRegistry.PlantInfoAgent;

            //Orchestration
            // =====================================================================================
            MagenticOrchestration = new Microsoft.SemanticKernel.Agents.Magentic.MagenticOrchestration(
                manager,
                chatModeratorAgent,
                projectInfoAgent,
                adafruitFeedAgent,
                plantInfoAgent
            )
            {
                ResponseCallback = ResponseCallback,
            };
        }

        public void ClearChatHistory()
        {
            ChatHistory.Clear();
        }

        public void SetChatHistory(ChatHistory userChatHistory)
        {
            ChatHistory = userChatHistory;
        }

        public string OutputAssistentResponseContent()
        {
            string combinedResponse = "";

            foreach (var assistentContent in ChatHistory.Where(c => c.Role == AuthorRole.Assistant))
            {
                combinedResponse = combinedResponse + $"#{assistentContent.Content}" + "\n\n";
            }

            return combinedResponse;
        }

        public async Task<string> GetResponse(string userPrompt)
        {
            InProcessRuntime runtime = new InProcessRuntime();

            await runtime.StartAsync();

            OrchestrationResult<string> result = await MagenticOrchestration.InvokeAsync(userPrompt, runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(180)); //Very important settings
            Console.WriteLine("//----------------//");
            Console.WriteLine(output);

            await runtime.RunUntilIdleAsync();

            return output;
        }
    }
}
#pragma warning enable
