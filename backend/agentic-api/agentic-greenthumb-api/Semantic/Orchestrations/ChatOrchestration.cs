using AgenticGreenthumbApi.Domain;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public abstract class ChatOrchestration
    {
        public string Name { get; set; } = string.Empty;

        public abstract string Type { get; set; }

        public ChatHistory ChatHistory { get; protected set; } = new();

        protected OrchestrationConfigTemplate OrchestrationConfig;

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
