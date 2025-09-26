using Microsoft.AspNetCore.Mvc;

namespace AgenticGreenthumbApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatCompletionController : ControllerBase
    {

        private readonly ILogger<ChatCompletionController> _logger;

        public ChatCompletionController(ILogger<ChatCompletionController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetChatHistory", Name = "GetChatHistory")]
        public IEnumerable<string> GetChatHistory()
        {
            return Enumerable.Range(1, 5).Select(index => "What a good cat")
            .ToArray();
        }

        [HttpGet("GetChatResponse", Name = "GetChatResponse")]
        public string GetChatResponse()
        {
            return "Hello World";
        }
    }
}
