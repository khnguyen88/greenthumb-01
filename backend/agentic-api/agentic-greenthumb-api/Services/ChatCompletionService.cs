using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
using AgenticGreenthumbApi.Factory;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Mappers;
using AgenticGreenthumbApi.Semantic.JointEnsembles;
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
        private readonly AgentRegistry _agentRegistry;
        private readonly JointEnsembleRegistry _jointEnsembleRegistry;

        public ChatCompletionService(ILogger<ChatCompletionService> logger, IConfiguration config, UserChatHistoryService userChatHistoryService, AgentFactory agentFactory, JointEnsembleFactory jointEnsembleFactory)
        {
            _logger = logger;
            _config = config;
            _userChatHistoryService = userChatHistoryService;
            _agentRegistry = agentFactory.GetAgentRegistry();
            _jointEnsembleRegistry = jointEnsembleFactory.GetOrchestrationRegistry();


        }

        public async Task<string> GetChatResponse(string userPrompt)
        {
            try
            {
                _jointEnsembleRegistry.JointEnsembles.TryGetValue("MainJointEnsemble", out ChatJointEnsemble jointEnsemble);

                ChatHistoryAgentThread agentThread = new();

                bool isUserHistoryExpired = _userChatHistoryService.IsUserChatHistoryRecordExpired(userName);

                if (!isUserHistoryExpired)
                {
                    ChatHistory userChatHistory = _userChatHistoryService.GetUserChatHistory(userName).ChatHistory ?? new ChatHistory();
                    _userChatHistoryService.PopulateAgentThread(agentThread, userChatHistory);
                }

                agentThread.ChatHistory.AddUserMessage(userPrompt);

                int numRetries = 1;
                int.TryParse(_config["Kernel:PromptRetries"], out numRetries);
                string output = string.Empty;

                while (output.IsNullOrEmpty() && numRetries > 0)
                {
                    Console.WriteLine("Response Retries: " + numRetries);

                    output = await jointEnsemble.GetResponse(userPrompt);
                    numRetries--;
                }

                Console.WriteLine("# of Content Before Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();

                string trueOrchestrationOutput = jointEnsemble.OutputAssistantResponseContent();
                Console.WriteLine(trueOrchestrationOutput);
                ChatHistoryHelper.ClearChatHistory(jointEnsemble.ChatHistory);
                ChatHistoryHelper.AppendChatMessage(agentThread.ChatHistory,AuthorRole.Assistant, output);
                _userChatHistoryService.AddUpdateUserChatHistory(userName, agentThread.ChatHistory);
                Console.WriteLine("# of Content After Updating User Chat History: " + agentThread.ChatHistory.Count);
                Console.WriteLine();

                return output;
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

        public async Task<ChatHistoryDto> GetPlantImageAnalysis(string userPrompt)
        {

            _agentRegistry.Agents.TryGetValue("GardenerAgent", out Agent gardenerAgent);

            _agentRegistry.Agents.TryGetValue("PlantHealthImageAnalyzerAgent", out Agent plantHealthImageAnalystAgent);

            ChatHistory chatMessages = new();

            //GET LOCAL IMAGE
            //var image = @"c:\temp\test.png";
            //var bytes = System.IO.File.ReadAllBytes(image);
            //var imageData = new ReadOnlyMemory<byte>(bytes);

            //GET URL IMAGE
            var imageURL = userPrompt.IsNullOrEmpty() ? "https://upload.wikimedia.org/wikipedia/commons/e/e5/Tomatoes_in_a_screen_house_02.jpg" : userPrompt;
            var imageData2 = await GetImageBytesFromUrlAsync(imageURL);


            var message = new ChatMessageContentItemCollection
            {
                new TextContent("What is the health of the plant in the image provided?"),
                new ImageContent(imageData2, "image/png")
            };

            chatMessages.AddUserMessage(message);

            ChatHistory copyChatHistory = new ChatHistory(chatMessages);

            ChatMessageContent response = await plantHealthImageAnalystAgent.InvokeAsync(chatMessages).FirstAsync();

            Console.WriteLine(response.Content.ToString());

            ChatMessageContent response2 = await gardenerAgent.InvokeAsync(copyChatHistory).FirstAsync();

            Console.WriteLine(response2.Content.ToString());

            ChatHistoryDto chatHistoryDto = new()
            {
                ChatMessages = ChatHistoryMapper.DomainToDtoMapper(new List<ChatMessageContent>() { response, response2 })
            };

            return chatHistoryDto;
        }

        public async Task<byte[]> GetImageBytesFromUrlAsync(string url)
        {
            using (var client = new HttpClient())
            {
                // Using GetByteArrayAsync is a concise way to get the data directly as a byte array
                client.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                    "AppleWebKit/537.36 (KHTML, like Gecko) " +
                    "Chrome/120.0.0.0 Safari/537.36");

                byte[] imageBytes = await client.GetByteArrayAsync(url);
                return imageBytes;
            }
        }
    }
}
#pragma warning restore