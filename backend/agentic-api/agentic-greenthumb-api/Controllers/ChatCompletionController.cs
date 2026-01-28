using AgenticGreenthumbApi.Dtos;
using AgenticGreenthumbApi.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;

namespace AgenticGreenthumbApi.Controllers
{
    [EnableCors("ChatPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatCompletionController : ControllerBase
    {

        private readonly ILogger<ChatCompletionController> _logger;
        private readonly ChatCompletionService _chatCompletionService;

        public ChatCompletionController(ILogger<ChatCompletionController> logger, ChatCompletionService chatCompletionService)
        {
            _logger = logger;
            _chatCompletionService = chatCompletionService;
        }


        [HttpPost("PostChatResponse", Name = "PostChatResponse")]
        public Task<ChatMessageDto> GetChatResponse([FromBody] ChatRequest chatRequest)
        {
            return _chatCompletionService.GetChatResponseMessage(chatRequest.Prompt);
        }

        [HttpPost("PostAllChatResponses", Name = "PostAllChatResponses")]
        public Task<ChatHistoryDto> GetAllChatResponses([FromBody] ChatRequest chatRequest)
        {
            return _chatCompletionService.GetChatHistory(chatRequest.Prompt);
        }

        [HttpPost("PartialChatResponses", Name = "PartialChatResponses")]
        public Task<ChatHistoryDto> GetPartialChatResponses([FromBody] ChatRequest chatRequest)
        {
            return _chatCompletionService.GetPartialChatHistory(chatRequest.Prompt);
        }

        [HttpPost("GetPlantImageAnalysis", Name = "GetPlantImageAnalysis")]
        public Task<ChatHistoryDto> GetPlantImageAnalysis([FromBody] string url)
        {
            return _chatCompletionService.GetPlantImageAnalysis(url);
        }
    }
}
