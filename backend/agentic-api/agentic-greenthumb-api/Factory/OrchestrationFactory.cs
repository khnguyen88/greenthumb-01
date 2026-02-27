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
                    ChatOrchestration chatOrchestration = BuildChatOrchestrationFromTemplate(template);

                    AddOrchestrationToRegistry(template.Name, chatOrchestration);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempts to generate orchestrations have falled due this error: {ex.Message}");
                }
            }
        }

        public ChatOrchestration BuildChatOrchestrationFromTemplate(OrchestrationConfigTemplate orchestrationConfigTemplate)
        {
            ChatOrchestration chatOrchestration;

            switch (orchestrationConfigTemplate.Type)
            {
                case OrchestrationType.Sequential:
                    {
                        chatOrchestration = new ChatSequentialOrchestration(orchestrationConfigTemplate, _agentRegistry.Agents.Values.ToArray());
                        break;
                    }
                case OrchestrationType.Concurrent:
                    {
                        chatOrchestration = new ChatConcurrentOrchestration(orchestrationConfigTemplate, _agentRegistry.Agents.Values.ToArray());
                        break;
                    }
                case OrchestrationType.Handoff:
                    {
                        chatOrchestration = new ChatHandoffOrchestration(orchestrationConfigTemplate, _agentRegistry.Agents.Values.ToArray());
                        break;
                    }
                case OrchestrationType.Magnetic:
                    {
                        chatOrchestration = new ChatMagenticOrchestration(orchestrationConfigTemplate, _kernelFactory.GetNewKernel(), _agentRegistry.Agents.Values.ToArray());
                        break;
                    }
                default:
                    {
                        chatOrchestration = new ChatSingleOrchestration(orchestrationConfigTemplate, _agentRegistry.Agents.Values.ToArray());
                        break;
                    }
            }

            return chatOrchestration;
        }

        public void AddOrchestrationToRegistry(string orchestrationName,  ChatOrchestration chatOrchestration)
        {
            _orchestrationRegistry.Orchestrations.TryAdd(orchestrationName, chatOrchestration);
        }

        public void UpdateOrchestrationInRegistry(string orchestrationName, OrchestrationConfigTemplate orchestrationConfigTemplate)
        {
            var isExists = _orchestrationRegistry.Orchestrations.ContainsKey(orchestrationName);

            if (isExists)
            {
                _orchestrationRegistry.Orchestrations[orchestrationName] = BuildChatOrchestrationFromTemplate(orchestrationConfigTemplate);
            }
        }

        public OrchestrationRegistry GetOrchestrationRegistry()
        {
            return _orchestrationRegistry;
        }
    }
}
