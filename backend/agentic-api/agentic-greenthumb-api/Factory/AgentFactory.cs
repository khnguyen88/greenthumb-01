using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;

namespace AgenticGreenthumbApi.Factory
{
    public class AgentFactory
    {
        private readonly IConfiguration _config;
        private readonly AgentRegistry _agentRegistry;

        public AgentFactory(IConfiguration config, AgentRegistry agentRegistry) {
            _config = config;
            _agentRegistry = agentRegistry;

            Initialize();
        }

        private void Initialize()
        {

        }

    }
}
