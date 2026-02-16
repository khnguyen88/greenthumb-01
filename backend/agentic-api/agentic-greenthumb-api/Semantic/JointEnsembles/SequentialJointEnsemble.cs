using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AgenticGreenthumbApi.Semantic.JointEnsembles
{
    public class SequentialJointEnsemble : ChatJointEnsemble
    {
        public SequentialJointEnsemble(JointEnsembleConfigTemplate jointEnsembleConfig, params ChatOrchestration[] orchestrations)
        {
            //Name
            Name = jointEnsembleConfig.Name;

            //Orchestration Config
            _jointEnsembleConfig = jointEnsembleConfig;

            //Chat History
            ChatHistory = [];

            //Orchestration List
            JointEnsemble = orchestrations.Where(o => _jointEnsembleConfig.ChatOrchestrationNames.Any(name => name == o.Name)).ToList();
        }

        public override async Task<string> GetResponse(string userPrompt){

            ChatHistoryHelper.AppendChatMessage(ChatHistory, AuthorRole.User, userPrompt);

            string response = userPrompt;

            foreach (var orchestration in JointEnsemble)
            {
                response = await orchestration.GetResponse(response);
                ChatHistoryHelper.AppendChatMessage(ChatHistory, AuthorRole.Assistant, response);
            }

            return response;
        }
    }
}
