using AgenticGreenthumbApi.Semantic.JointEnsembles;
using AgenticGreenthumbApi.Semantic.Orchestrations;

namespace AgenticGreenthumbApi.Domain
{
    public class JointEnsembleRegistry
    {
        public Dictionary<string, ChatJointEnsemble> JointEnsembles { get; set; } = new();
    }
}
