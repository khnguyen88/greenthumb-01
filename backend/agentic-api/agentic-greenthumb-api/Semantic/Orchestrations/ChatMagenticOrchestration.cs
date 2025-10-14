using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatMagenticOrchestration
    {
        public Microsoft.SemanticKernel.Agents.Magentic.MagenticOrchestration MagenticOrchestration { get; set; }
        public ChatHistory ChatHistory {get; set;} = [];

        public ChatMagenticOrchestration(ProjectInfoAgentRegistry projectInfoAgentRegistry, AdafruitFeedAgentRegistry adafruitFeedAgentRegistry)
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
            var projectInfo = projectInfoAgentRegistry.ProjectInfoAgent;
            var adafruitFeedAgent = adafruitFeedAgentRegistry.AdafruitFeedAgent;

            //Orchestration
            // =====================================================================================
            MagenticOrchestration = new Microsoft.SemanticKernel.Agents.Magentic.MagenticOrchestration(
                manager,
                projectInfo,
                adafruitFeedAgent
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

            OrchestrationResult<string> result = await MagenticOrchestration.InvokeAsync(userPrompt + "and then exit out of stream", runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(180)); //Very important settings
            Console.WriteLine("//----------------//");
            Console.WriteLine(output);

            await runtime.StopAsync();

            return output;
        }
    }
}
#pragma warning enable
