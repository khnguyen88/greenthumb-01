using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Runtime;

namespace AgenticGreenthumbApi.Domain
{
    public class AgentRegistry
    {
        public Dictionary<string, Agent> Agents { get; set; } = new();
    }
}
