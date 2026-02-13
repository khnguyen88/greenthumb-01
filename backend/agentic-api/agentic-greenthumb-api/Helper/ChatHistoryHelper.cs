using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AgenticGreenthumbApi.Helper
{
    public static class ChatHistoryHelper
    {
        public static void ClearChatHistory(ChatHistory chatHistory)
        {
            chatHistory.Clear();
        }

        public static void SetChatHistory(ChatHistory newChatHistory, ChatHistory existingChatHistory)
        {
            newChatHistory = existingChatHistory ?? new ChatHistory();
        }

        public static void JoinChatHistories(ChatHistory existingchatHistory, ChatHistory recentChatHistory)
        {
            existingchatHistory.AddRange(recentChatHistory);
        }

        public static void AppendChatHistoryContent(ChatHistory existingchatHistory, ChatMessageContent chatMessegeContent) {
        
            existingchatHistory.Add(chatMessegeContent);
        }


        public static void AppendChatResponseMessage(ChatHistory existingchatHistory, string output)
        {
            if (!existingchatHistory.Select(x => x.Content).ToList().Contains(output))
            {
                existingchatHistory.AddAssistantMessage(output);
            }

        }

        public static void AppendChatMessage(ChatHistory chatHistory, AuthorRole role, string output)
        {
                chatHistory.AddMessage(role, output);
        }
    }
}
