using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.ML.OnnxRuntimeGenAI;
using Microsoft.SemanticKernel.Agents;

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
        }

        public static async Task<OrchestrationFactory> CreateAsync(IConfiguration config, KernelFactory kernelFactory, AgentFactory agentFactory, OrchestrationRegistry orchestrationRegistry)
        {
            await agentFactory.InitializeAsync();

            var orchestrationFactory = new OrchestrationFactory(config, kernelFactory, agentFactory, orchestrationRegistry);

            await orchestrationFactory.InitializeAsync();

            return orchestrationFactory;
        }

        public async Task InitializeAsync()
        {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var orchestrationTemplateSubdirectories = templateSection.GetSection("Orchestration")
                .GetSection("SubDirectories")
                .Get<string[]>();

            List<OrchestrationConfigTemplate> orchestrationTemplates = await FileHelper.GetTemplateFilesAsync<OrchestrationConfigTemplate>(orchestrationTemplateSubdirectories);

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
