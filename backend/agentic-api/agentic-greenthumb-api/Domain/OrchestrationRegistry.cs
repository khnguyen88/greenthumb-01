using AgenticGreenthumbApi.Semantic.Orchestrations;
using Microsoft.SemanticKernel.Agents;

namespace AgenticGreenthumbApi.Domain
{
    public class OrchestrationRegistry
    {
        public Dictionary<string, ChatOrchestration> Orchestrations { get; set; } = new();
    }
}
