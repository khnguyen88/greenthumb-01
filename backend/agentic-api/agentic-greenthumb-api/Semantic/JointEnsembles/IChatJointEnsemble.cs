using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AgenticGreenthumbApi.Semantic.JointEnsembles
{
    public interface IChatJointEnsemble
    {
        ChatHistory ChatHistory { get; set; }

        List<ChatOrchestration> JointEnsemble { get; set; }

        string OutputAssistantResponseContent();

        Task<string> GetResponse(string userPrompt);
    }
}
