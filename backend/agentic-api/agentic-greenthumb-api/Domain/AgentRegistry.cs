using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Runtime;

namespace AgenticGreenthumbApi.Domain
{
    public class AgentRegistry
    {
        public Agent[] Agents { get; set; } = [];
    }
}
