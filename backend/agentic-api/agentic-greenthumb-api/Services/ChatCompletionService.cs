using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using AgenticGreenthumbApi.Semantic.Plugins;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors;
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
            ChatAgentRegistry chatAgentRegistry = new ChatAgentRegistry(new ProjectInfoPlugin(), new AdafruitPlugin(_adafruitService));
            AdafruitFeedAgentRegistry adafruitFeedAgentRegistry = new AdafruitFeedAgentRegistry(new AdafruitPlugin(_adafruitService));

            ChatHistoryAgent chatHistoryAgent = chatAgentRegistry.ChatCompletionAgent;
            //ChatOrchestration chatOrchestration = new ChatOrchestration(chatAgentRegistry, adafruitFeedAgentRegistry);

            ChatHistoryAgentThread agentThread = new();

            bool isUserHistoryExpired = _userChatHistoryService.IsUserChatHistoryRecordExpired(userName);

            if (!isUserHistoryExpired)
            {
                ChatHistory userChatHistory = _userChatHistoryService.GetUserChatHistory(userName).ChatHistory ?? new ChatHistory();
                _userChatHistoryService.PopulateAgentThread(agentThread, userChatHistory);
            }

            //agentThread.ChatHistory.AddUserMessage(userPrompt);

            //InProcessRuntime runtime = new InProcessRuntime();

            //await runtime.StartAsync();

            //OrchestrationResult<string> result = await chatOrchestration.MagenticOrchestration.InvokeAsync(userPrompt, runtime);
            //string output = await result.GetValueAsync();
            //Console.WriteLine(output.ToString());
            //agentThread.ChatHistory.AddAssistantMessage(output);

            //await runtime.RunUntilIdleAsync();


            // AGENT BASIC
            ChatMessageContent message = await chatAgentRegistry.ChatCompletionAgent.InvokeAsync(userPrompt, agentThread).FirstAsync();

            _userChatHistoryService.AddUpdateUserChatHistory(userName, agentThread.ChatHistory);



            return message.Content ?? "The chat agent was unable to respond to your prompt";
        }
    }
}
#pragma warning restore