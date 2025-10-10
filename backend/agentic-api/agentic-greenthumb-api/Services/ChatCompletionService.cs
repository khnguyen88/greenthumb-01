using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
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
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors;
using Microsoft.SemanticKernel.PromptTemplates;
using Spectre.Console;
using System.Text.Json;


namespace AgenticGreenthumbApi.Services
{
    public class ChatCompletionService
    {
        private readonly ILogger<ChatCompletionService> _logger;
        private readonly UserChatHistoryService _userChatHistoryService;
        private readonly ChatCompletionPlugin _chatCompletionPlugin;
        private const string userName = "nguyekhi"; //Eventually pass this in

        public ChatCompletionService(ILogger<ChatCompletionService> logger, UserChatHistoryService userChatHistoryService, ChatCompletionPlugin chatCompletionPlugin)
        {
            _logger = logger;
            _userChatHistoryService = userChatHistoryService;
            _chatCompletionPlugin = chatCompletionPlugin;
        }

        public async Task<string> GetChatResponse(string userPrompt)
        {
            ChatAgentRegistry chatAgentRegistry = new ChatAgentRegistry(_chatCompletionPlugin);

            ChatHistoryAgentThread agentThread = new();

            bool isUserHistoryExpired = _userChatHistoryService.IsUserChatHistoryRecordExpired(userName);

            if (!isUserHistoryExpired)
            {
                ChatHistory userChatHistory = _userChatHistoryService.GetUserChatHistory(userName).ChatHistory ?? new ChatHistory();
                _userChatHistoryService.PopulateAgentThread(agentThread, userChatHistory);
            }

            ChatMessageContent message = await chatAgentRegistry.ChatCompletionAgent.InvokeAsync(userPrompt, agentThread).FirstAsync();

            Console.WriteLine(message.ToString());

            _userChatHistoryService.AddUpdateUserChatHistory(userName, agentThread.ChatHistory);

            await agentThread.DeleteAsync();

            return message.Content ?? "The chat agent was unable to respond to your prompt";
        }

        


    }
}
