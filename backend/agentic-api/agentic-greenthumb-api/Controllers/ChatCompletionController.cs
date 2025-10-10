using AgenticGreenthumbApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace AgenticGreenthumbApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatCompletionController : ControllerBase
    {

        private readonly ILogger<ChatCompletionController> _logger;
        private readonly ChatCompletionService _chatCompletionService;

        public ChatCompletionController(ILogger<ChatCompletionController> logger, ChatCompletionService chatCompletionService)
        {
            _logger = logger;
            _chatCompletionService = chatCompletionService;
        }

        [HttpGet("GetChatHistory", Name = "GetChatHistory")]
        public IEnumerable<string> GetChatHistory()
        {
            return Enumerable.Range(1, 5).Select(index => "What a good cat")
            .ToArray();
        }

        [HttpGet("GetChatResponse", Name = "GetChatResponse")]
        public async Task<string> GetChatResponse(string prompt)
        {
            return await _chatCompletionService.GetChatResponse(prompt);
        }
    }
}
