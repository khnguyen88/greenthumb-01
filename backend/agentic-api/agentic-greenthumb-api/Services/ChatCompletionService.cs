using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
using AgenticGreenthumbApi.Factory;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Mappers;
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
        private readonly IConfiguration _config;
        private readonly UserChatHistoryService _userChatHistoryService;
        private readonly AdafruitService _adafruitService;
        private const string userName = "nguyekhi"; //Eventually pass this in
        private KernelFactory _kernelFactory;

        public ChatCompletionService(ILogger<ChatCompletionService> logger, IConfiguration config, UserChatHistoryService userChatHistoryService, AdafruitService adafruitService, AdafruitFeedAgentRegistry adafruitFeedAgentRegistry, KernelFactory kernelFactory)
        {
            _logger = logger;
            _config = config;
            _userChatHistoryService = userChatHistoryService;
            _adafruitService = adafruitService;
        }

        public async Task<string> GetChatResponse(string userPrompt)
        {
            ChatModeratorAgentRegistry chatModeratorAgentRegistry = new ChatModeratorAgentRegistry();
            ProjectInfoAgentRegistry projectInfoAgentRegistry = new ProjectInfoAgentRegistry(new ProjectInfoPlugin());
            AdafruitFeedAgentRegistry adafruitFeedAgentRegistry = new AdafruitFeedAgentRegistry(new AdafruitPlugin(_adafruitService));
            PlantInfoAgentRegistry plantInfoAgentRegistry = new PlantInfoAgentRegistry();
            ChatEditorAgentRegistry chatEditorRegistry = new ChatEditorAgentRegistry(_kernelFactory);

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

                int numRetries = 1;
                int.TryParse(_config["Kernel:PromptRetries"], out numRetries);
                string output = string.Empty;

                while (output == string.Empty && numRetries > 0)
                {
                    Console.WriteLine("Response Retries: " + numRetries);

                    //output = await chatMagenticOrchestration.GetResponse(userPrompt);
                    output = await chatOrchestration.GetResponse(userPrompt);
                    numRetries--;
                }

                Console.WriteLine("# of Content Before Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();


                string trueOrchestrationOutput =  chatOrchestration.OutputAssistentResponseContent();
                Console.WriteLine(trueOrchestrationOutput);
                chatOrchestration.ClearChatHistory();

                ChatMessageContent santitizedOutput = await chatEditorRegistry.ChatEditorAgent.InvokeAsync(trueOrchestrationOutput).FirstAsync();

                agentThread.ChatHistory.Add(santitizedOutput);
                _userChatHistoryService.AddUpdateUserChatHistory(userName, agentThread.ChatHistory);
                Console.WriteLine("# of Content After Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();

                return santitizedOutput.Content;
            }
            catch(Exception ex)
            {
                return ex.Message + "\n\n" + "Please adjust your prompt and try again.";
            }
        }

        public async Task<ChatMessageDto> GetChatResponseMessage(string userPrompt)
        {
            var response = await GetChatResponse(userPrompt);
            Console.WriteLine(response);

            return new ChatMessageDto() { Role = "assistant", Message = response };
        }

        public async Task<ChatHistoryDto> GetChatHistory(string userPrompt)
        {
            _ = await GetChatResponse(userPrompt);

            ChatHistoryDto chatHistoryDto = new()
            {
                ChatMessages = ChatHistoryMapper.DomainToDtoMapper(_userChatHistoryService.GetUserChatHistory(userName).ChatHistory.ToList())
            };

            return chatHistoryDto;

        }

        public async Task<ChatHistoryDto> GetPartialChatHistory(string userPrompt)
        {
            //This is to simulate a scenerio where I run this on cloud run. Likely the history will not persist since it's on a per REST API call instance. If no one is using it, the application gets shut down.

            _ = await GetChatResponse(userPrompt);

            ChatHistoryDto chatHistoryDto = new()
            {
                ChatMessages = ChatHistoryMapper.DomainToDtoMapper(_userChatHistoryService.GetUserChatHistory(userName).ChatHistory.TakeLast(2).ToList())
            };

            return chatHistoryDto;

        }
    }
}
#pragma warning restore