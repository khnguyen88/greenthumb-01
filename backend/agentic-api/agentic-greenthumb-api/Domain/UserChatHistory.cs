using Microsoft.SemanticKernel.ChatCompletion;

namespace AgenticGreenthumbApi.Domain
{
    public class UserChatHistory
    {
        public string Username { get; set; }
        public DateTime SessionDateTime { get; set; }
        public ChatHistory? ChatHistory { get; set; }
    }
}
