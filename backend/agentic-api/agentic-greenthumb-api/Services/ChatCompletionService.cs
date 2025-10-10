using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors;
using Microsoft.SemanticKernel.PromptTemplates;
using Microsoft.SemanticKernel.Agents;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;
using Microsoft.KernelMemory.DocumentStorage.DevTools;
using Microsoft.KernelMemory.FileSystem.DevTools;
using Microsoft.KernelMemory.MemoryStorage.DevTools;
using Spectre.Console;
using System.Text.Json;
using AgenticGreenthumbApi.Helper;


namespace AgenticGreenthumbApi.Services
{
    public class ChatCompletionService
    {
        private readonly ILogger<ChatCompletionService> _logger;
        private readonly KernelFactoryHelper _kernelFactoryService;
        private readonly UserChatHistoryService _userChatHistoryService;

        public ChatCompletionService(ILogger<ChatCompletionService> logger, KernelFactoryHelper kernelFactoryService, UserChatHistoryService userChatHistoryService)
        {
            _logger = logger;
            _kernelFactoryService = kernelFactoryService;
            _userChatHistoryService = userChatHistoryService;
        }

        


    }
}
