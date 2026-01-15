using Microsoft.SemanticKernel.Agents;

namespace AgenticGreenthumbApi.Domain
{
    public class AgentRegistry
    {
        public List<Agent> Agents { get; set; } = new();
    }
}
