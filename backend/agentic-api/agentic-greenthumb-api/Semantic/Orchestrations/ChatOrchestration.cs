using AgenticGreenthumbApi.Domain;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public abstract class ChatOrchestration
    {
        public OrchestrationConfigTemplate OrchestrationConfig { get; set; } = new();

        public ChatHistory ChatHistory { get; protected set; } = new();



        public void ClearChatHistory()
        {
            ChatHistory.Clear();
        }

        public void SetChatHistory(ChatHistory userChatHistory)
        {
            ChatHistory = userChatHistory ?? new ChatHistory();
        }

        public void AppendChatHistory(string output)
        {
            if (!ChatHistory.Select(x => x.Content).ToList().Contains(output))
            {
                ChatHistory.AddAssistantMessage(output);
            }

        }

        public string OutputAssistantResponseContent()
        {
            var sb = new StringBuilder();

            foreach (var msg in ChatHistory)
            {
                sb.Append('#')
                  .Append(msg.Content)
                  .Append("\n\n");
            }

            return sb.ToString();
        }

        public abstract Task<string> GetResponse(string userPrompt);

        public ValueTask ResponseCallback(ChatMessageContent response)
        {
            Console.WriteLine();
            Console.WriteLine($"# {response.Role} - {response.AuthorName}: {response.Content}");
            Console.WriteLine();
            ChatHistory.Add(response);

            return ValueTask.CompletedTask;
        }
    }
}
