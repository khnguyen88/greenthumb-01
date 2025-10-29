using StackExchange.Redis;

namespace AgenticGreenthumbApi.Dtos
{
    public class ChatHistoryDto
    {
        public List<ChatMessageDto> ChatMessages { get; set; } = new List<ChatMessageDto>();
    }

    public class ChatMessageDto
    {
        public string Role { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
    public class ChatRequest
    {
        public string Prompt { get; set; }
    }


}
