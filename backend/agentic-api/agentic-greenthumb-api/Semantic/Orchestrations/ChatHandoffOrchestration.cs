using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.Handoff;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;


#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatHandoffOrchestration
    {
        public HandoffOrchestration HandoffOrchestration { get; set; }
        public ChatHistory ChatHistory { get; set; } = [];

        public ChatHandoffOrchestration(ChatModeratorAgentRegistry chatModeratorAgentRegistry, ProjectInfoAgentRegistry projectInfoAgentRegistry, AdafruitFeedAgentRegistry adafruitFeedAgentRegistry, PlantInfoAgentRegistry plantInfoAgentRegistry)
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

            //Agents
            var chatModeratorAgent = chatModeratorAgentRegistry.ChatModeratorAgent;
            var projectInfoAgent = projectInfoAgentRegistry.ProjectInfoAgent;
            var adafruitFeedAgent = adafruitFeedAgentRegistry.AdafruitFeedAgent;
            var plantInfoAgent = plantInfoAgentRegistry.PlantInfoAgent;

            //Handoff Setup
            OrchestrationHandoffs handoffs = OrchestrationHandoffs
                .StartWith(chatModeratorAgent)
                .Add(chatModeratorAgent, projectInfoAgent, adafruitFeedAgent, plantInfoAgent)
                .Add(projectInfoAgent, chatModeratorAgent, "Transfer to this agent if the prompts is not related to IoT gardening system or project information.")
                .Add(adafruitFeedAgent, chatModeratorAgent, "Transfer to this agent if the prompts is is not related to sensor feed data (humidity, temperature, etc.) or adafruit IO related.")
                .Add(plantInfoAgent, chatModeratorAgent, "Transfer to this agent if the prompts is not related plant information or plant related questions or prompts.");


            //Handoff Orchestration
            HandoffOrchestration = new HandoffOrchestration(handoffs, chatModeratorAgent, projectInfoAgent, adafruitFeedAgent, plantInfoAgent)
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

            OrchestrationResult<string> result = await HandoffOrchestration.InvokeAsync(userPrompt, runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(180)); //Very important settings
            Console.WriteLine("//----------------//");
            Console.WriteLine(output);

            await runtime.RunUntilIdleAsync();

            return output;
        }
    }
}
#pragma warning restore
