using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

namespace AgenticGreenthumbApi.Semantic.JointEnsembles
{
    public abstract class ChatJointEnsemble
    {
        public string Name { get; set; }
        public ChatHistory ChatHistory { get; set; }
        
        public List<ChatOrchestration> JointEnsemble { get; set; }

        protected JointEnsembleConfigTemplate _jointEnsembleConfig;

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
    }
}
