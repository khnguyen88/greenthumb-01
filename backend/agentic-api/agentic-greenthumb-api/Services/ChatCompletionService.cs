using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.Handoff;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates;
using Spectre.Console;
using System.Text.Json;


#pragma warning disable
namespace AgenticGreenthumbApi.Services
{
    public class ChatCompletionService
    {
        private readonly ILogger<ChatCompletionService> _logger;
        private readonly UserChatHistoryService _userChatHistoryService;
        private readonly AdafruitService _adafruitService;
        private const string userName = "nguyekhi"; //Eventually pass this in

        public ChatCompletionService(ILogger<ChatCompletionService> logger, UserChatHistoryService userChatHistoryService, AdafruitService adafruitService)
        {
            _logger = logger;
            _userChatHistoryService = userChatHistoryService;
            _adafruitService = adafruitService;
        }

        public async Task<string> GetChatResponse(string userPrompt)
        {
            ProjectInfoAgentRegistry chatAgentRegistry = new ProjectInfoAgentRegistry(new ProjectInfoPlugin(), new AdafruitPlugin(_adafruitService));
            AdafruitFeedAgentRegistry adafruitFeedAgentRegistry = new AdafruitFeedAgentRegistry(new AdafruitPlugin(_adafruitService));

            ChatHistoryAgent chatHistoryAgent = chatAgentRegistry.ProjectInfoAgent;
            //ChatOrchestration chatOrchestration = new ChatOrchestration(chatAgentRegistry, adafruitFeedAgentRegistry);

            ChatHistoryAgentThread agentThread = new();

            bool isUserHistoryExpired = _userChatHistoryService.IsUserChatHistoryRecordExpired(userName);

            if (!isUserHistoryExpired)
            {
                ChatHistory userChatHistory = _userChatHistoryService.GetUserChatHistory(userName).ChatHistory ?? new ChatHistory();
                _userChatHistoryService.PopulateAgentThread(agentThread, userChatHistory);
            }

            //MAGENTIC (COMMMENT ONE OR THE OTHER OUT)
            MagenticOrchestration orchestration;
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
            var projectInfoAgent = chatAgentRegistry.ProjectInfoAgent;
            var adafruitFeedAgent = adafruitFeedAgentRegistry.AdafruitFeedAgent;

            //Orchestration
            orchestration = new MagenticOrchestration(
                manager,
                projectInfoAgent,
                adafruitFeedAgent
            )
            {
                ResponseCallback = ResponseCallback,
            };

            agentThread.ChatHistory.AddUserMessage(userPrompt);

            try
            {
                InProcessRuntime runtime = new InProcessRuntime();

                await runtime.StartAsync();

                OrchestrationResult<string> result = await orchestration.InvokeAsync(userPrompt + " and then exit out of stream", runtime);
                string output = await result.GetValueAsync(TimeSpan.FromSeconds(120));
                Console.WriteLine("//----------------//");
                Console.WriteLine(output);
                agentThread.ChatHistory.AddAssistantMessage(output);

                await runtime.StopAsync();


                // AGENT BASIC
                //ChatMessageContent message = await chatAgentRegistry.ChatCompletionAgent.InvokeAsync(userPrompt, agentThread).FirstAsync();

                _userChatHistoryService.AddUpdateUserChatHistory(userName, agentThread.ChatHistory);

                return output;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }



        }
    }
}
#pragma warning restore