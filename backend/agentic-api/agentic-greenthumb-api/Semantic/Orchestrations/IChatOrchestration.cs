using AgenticGreenthumbApi.Domain;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public interface IChatOrchestration
    {
        OrchestrationConfigTemplate OrchestrationConfig { get; set; }

        ChatHistory ChatHistory { get; set; }

        void ClearChatHistory();

        void SetChatHistory(ChatHistory userChatHistory);

        string OutputAssistantResponseContent();

        Task<string> GetResponse(string userPrompt);
    }
}
