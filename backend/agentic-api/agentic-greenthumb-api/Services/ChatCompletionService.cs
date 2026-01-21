using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
using AgenticGreenthumbApi.Factory;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Mappers;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.IdentityModel.Tokens;
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
using AgentFactory = AgenticGreenthumbApi.Factory.AgentFactory;


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
        private readonly AgentFactory _agentFactory;
        private readonly AgentRegistry _agentRegistry;

        public ChatCompletionService(ILogger<ChatCompletionService> logger, IConfiguration config, UserChatHistoryService userChatHistoryService, AgentFactory agentFactory)
        {
            _logger = logger;
            _config = config;
            _userChatHistoryService = userChatHistoryService;
            _agentFactory = agentFactory;
            _agentRegistry = agentFactory.GetAgentRegistry();


        }

        public async Task<string> GetChatResponse(string userPrompt)
        {
            OrchestrationConfig orchestrationConfig = GetOrchestrationConfig(_config);

            Agent chatEditorAgent = _agentRegistry.Agents.FirstOrDefault(a => a.Name == "ChatEditorAgent");
              
            ChatHandoffOrchestration chatOrchestration = new ChatHandoffOrchestration(orchestrationConfig, _agentRegistry.Agents);

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

                while (output.IsNullOrEmpty() && numRetries > 0)
                {
                    Console.WriteLine("Response Retries: " + numRetries);

                    //output = await chatMagenticOrchestration.GetResponse(userPrompt);
                    output = await chatOrchestration.GetResponse(userPrompt);
                    numRetries--;
                }

                Console.WriteLine("# of Content Before Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();


                string trueOrchestrationOutput = chatOrchestration.OutputAssistentResponseContent();
                Console.WriteLine(trueOrchestrationOutput);
                chatOrchestration.ClearChatHistory();

                ChatMessageContent santitizedOutput = await chatEditorAgent.InvokeAsync(trueOrchestrationOutput).FirstAsync();

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

        public OrchestrationConfig GetOrchestrationConfig(IConfiguration config)
        {
            IConfigurationSection templateSection = config.GetSection("Template");

            var agentTemplateSubdirectories = templateSection.GetSection("Orchestration")
                .GetSection("SubDirectories")
                .Get<string[]>();

            var orchestrationConfigFileName = templateSection.GetSection("Orchestration")
                .GetSection("Filename")
                .Get<string>();


            var configJson = FileReaderHelper.GetFileFromDirectory(agentTemplateSubdirectories, orchestrationConfigFileName);

            return JsonSerializer.Deserialize<OrchestrationConfig>(configJson);
        }
    }
}
#pragma warning restore