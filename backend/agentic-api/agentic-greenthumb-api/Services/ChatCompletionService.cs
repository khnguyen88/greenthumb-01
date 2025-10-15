using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Semantic.Orchestrations;
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
            ChatModeratorAgentRegistry chatModeratorAgentRegistry = new ChatModeratorAgentRegistry();
            ProjectInfoAgentRegistry projectInfoAgentRegistry = new ProjectInfoAgentRegistry(new ProjectInfoPlugin());
            AdafruitFeedAgentRegistry adafruitFeedAgentRegistry = new AdafruitFeedAgentRegistry(new AdafruitPlugin(_adafruitService));
            PlantInfoAgentRegistry plantInfoAgentRegistry = new PlantInfoAgentRegistry();
            ChatEditorAgentRegistry chatEditorRegistry = new ChatEditorAgentRegistry();

            //ChatMagenticOrchestration chatOrchestration = new ChatMagenticOrchestration(chatModeratorAgentRegistry, projectInfoAgentRegistry, adafruitFeedAgentRegistry, plantInfoAgentRegistry);
            ChatHandoffOrchestration chatOrchestration = new ChatHandoffOrchestration(chatModeratorAgentRegistry, projectInfoAgentRegistry, adafruitFeedAgentRegistry, plantInfoAgentRegistry);

            ChatHistoryAgentThread agentThread = new();

            bool isUserHistoryExpired = _userChatHistoryService.IsUserChatHistoryRecordExpired(userName);

            if (!isUserHistoryExpired)
            {
                ChatHistory userChatHistory = _userChatHistoryService.GetUserChatHistory(userName).ChatHistory ?? new ChatHistory();
                _userChatHistoryService.PopulateAgentThread(agentThread, userChatHistory);
            }

            agentThread.ChatHistory.AddUserMessage(userPrompt);

            try
            {
                //string output = await chatMagenticOrchestration.GetResponse(userPrompt);
                string output = await chatOrchestration.GetResponse(userPrompt);

                Console.WriteLine("# of Content Before Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();


                string trueOrchestrationOutput =  chatOrchestration.OutputAssistentResponseContent();
                Console.WriteLine(trueOrchestrationOutput);
                chatOrchestration.ClearChatHistory();

                ChatMessageContent santitizedOutput = await chatEditorRegistry.ChatEditorAgent.InvokeAsync(trueOrchestrationOutput, agentThread).FirstAsync();

                _userChatHistoryService.AppendUserChatHistory(agentThread.ChatHistory, chatOrchestration.ChatHistory);
                _userChatHistoryService.AddUpdateUserChatHistory(userName, agentThread.ChatHistory);
                Console.WriteLine("# of Content After Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();

                return santitizedOutput.Content;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }



        }
    }
}
#pragma warning restore