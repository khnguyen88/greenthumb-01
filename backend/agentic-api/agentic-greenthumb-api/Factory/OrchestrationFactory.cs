using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace AgenticGreenthumbApi.Factory
{
    public class OrchestrationFactory
    {
        private readonly IConfiguration _config;
        private readonly KernelFactory _kernelFactory;
        private readonly AgentRegistry _agentRegistry;
        private readonly OrchestrationRegistry _orchestrationRegistry;

        public OrchestrationFactory(IConfiguration config, KernelFactory kernelFactory, AgentFactory agentFactory, OrchestrationRegistry orchestrationRegistry)
        {
            _config = config;
            _kernelFactory = kernelFactory;
            _agentRegistry = agentFactory.GetAgentRegistry();
            _orchestrationRegistry = orchestrationRegistry;

            Initialize(_agentRegistry, _orchestrationRegistry);
        }

        private void Initialize(AgentRegistry agentRegistry, OrchestrationRegistry orchestrationRegistry)
        {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var agentTemplateSubdirectories = templateSection.GetSection("Orchestration")
                .GetSection("SubDirectories")
                .Get<string[]>();

            List<OrchestrationConfigTemplate> orchestrationTemplates = FileReaderHelper.GetTemplateFiles<OrchestrationConfigTemplate>(agentTemplateSubdirectories);

            foreach (var template in orchestrationTemplates)
            {
                try
                {
                    ChatOrchestration chatOrchestration;

                    switch (template.Type)
                    {
                        case OrchestrationType.Sequential:
                            {
                                chatOrchestration = new ChatSequentialOrchestration(template, _agentRegistry.Agents.Values.ToArray());
                                break;
                            }
                        case OrchestrationType.Concurrent:
                            {
                                chatOrchestration = new ChatConcurrentOrchestration(template, _agentRegistry.Agents.Values.ToArray());
                                break;
                            }
                        case OrchestrationType.Handoff:
                            {
                                chatOrchestration = new ChatHandoffOrchestration(template, _agentRegistry.Agents.Values.ToArray());
                                break;
                            }
                        case OrchestrationType.Magnetic:
                            {
                                chatOrchestration = new ChatMagenticOrchestration(template, _kernelFactory.GetNewKernel(), _agentRegistry.Agents.Values.ToArray());
                                break;
                            }
                        default:
                            {
                                chatOrchestration = new ChatSingleOrchestration(template, _agentRegistry.Agents.Values.ToArray());
                                break;
                            }
                    }

                    _orchestrationRegistry.Orchestrations.TryAdd(template.Name, chatOrchestration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempts to generate orchestrations have falled due this error: {ex.Message}");
                }
            }
        }

        public OrchestrationRegistry GetOrchestrationRegistry()
        {
            return _orchestrationRegistry;
        }
    }
}
