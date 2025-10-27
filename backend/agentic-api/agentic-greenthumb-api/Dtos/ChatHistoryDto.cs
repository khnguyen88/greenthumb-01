using StackExchange.Redis;

namespace AgenticGreenthumbApi.Dtos
{
    public class ChatHistoryDto
    {
        public string Role { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
