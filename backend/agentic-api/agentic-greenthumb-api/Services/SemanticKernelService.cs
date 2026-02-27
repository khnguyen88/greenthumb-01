using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Factory;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Providers;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Microsoft.ML.OnnxRuntimeGenAI;
using NRedisStack.Search;
using System.Text.Json;

namespace AgenticGreenthumbApi.Services
{
    public class SemanticKernelService
    {
        private readonly ILogger<ChatCompletionService> _logger;
        private readonly IConfiguration _config;
        private readonly IStorageProvider _storageProvider;
        private AgentRegistry _agentRegistry;
        private OrchestrationRegistry _orchestrationRegistry;
        private JointEnsembleRegistry _jointEnsembleRegistry;
        private readonly IFactoryProvider<AgentFactory> _agentFactoryProvider;
        private readonly IFactoryProvider<OrchestrationFactory> _orchestrationFactoryProvider;
        private readonly IFactoryProvider<JointEnsembleFactory> _jointEnsembleFactoryProvider;

        public SemanticKernelService(ILogger<ChatCompletionService> logger, IConfiguration config, LocalStorageProvider localStorageProvider, IFactoryProvider<AgentFactory> agentFactoryProvider, IFactoryProvider<OrchestrationFactory> orcestrationFactoryProvider, IFactoryProvider<JointEnsembleFactory> jointEnsembleFactoryProvider)
        {
            _logger = logger;
            _config = config;
            _storageProvider = localStorageProvider;
            _agentFactoryProvider = agentFactoryProvider;
            _orchestrationFactoryProvider = orcestrationFactoryProvider;
            _jointEnsembleFactoryProvider = jointEnsembleFactoryProvider;
        }

        public async Task BuildRegistry()
        {
            var agentFactory = await _agentFactoryProvider.GetFactoryAsync();
            _agentRegistry = agentFactory.GetAgentRegistry();

            var orchestrationFactory = await _orchestrationFactoryProvider.GetFactoryAsync();
            _orchestrationRegistry = orchestrationFactory.GetOrchestrationRegistry();

            var jointEnsembleFactory = await _jointEnsembleFactoryProvider.GetFactoryAsync();
            _jointEnsembleRegistry = jointEnsembleFactory.GetJointEnsembleRegistry();
        }

        public async Task UpdateAgentRegistry(AgentConfigTemplate agentConfig)        
        {
            await BuildRegistry();

            var agentFactory = await _agentFactoryProvider.GetFactoryAsync();
            Console.WriteLine("Agent Registry Count Before Update: " + _agentRegistry.Agents.Count);
            await agentFactory.BuildAgentAsync(agentConfig);
            Console.WriteLine("Agent Registry Count After Update: " + _agentRegistry.Agents.Count);
        }

        public async Task SaveAgentConfigFile(AgentConfigTemplate agentConfigTemplate)
        {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var agentTemplateSubdirectories = templateSection
                .GetSection("Agent")
                .GetSection("SubDirectories")
                .Get<string[]>();

            var jsonString = JsonSerializer.Serialize(agentConfigTemplate);
            Console.WriteLine(jsonString);

            var directoryPath = FileHelper.BuildPathFromProjectDirectory(agentTemplateSubdirectories);
            var saveFilePath = FileHelper.BuildFilePath(directoryPath, agentConfigTemplate.Filename);

            await _storageProvider.SaveFileAsync<AgentConfigTemplate>(saveFilePath, agentConfigTemplate);
            Console.WriteLine("Agent has been saved, successfully!");
        }

        public async Task UpdateOrchestrationRegistryAddAgents(AgentConfigTemplate agentConfig, string orchestrationConfigName)
        {
            await BuildRegistry();

            var agentRegistryUpdateCheck = _agentRegistry.Agents.ContainsKey(agentConfig.Name);
            var orchestrationRegistryExistCheck = _orchestrationRegistry.Orchestrations.TryGetValue(orchestrationConfigName, out ChatOrchestration? chatOrchestration);


            if (agentRegistryUpdateCheck)
            {
                if (orchestrationRegistryExistCheck || chatOrchestration is not null)
                {
                    var orchestrationTemplateConfig = chatOrchestration.GetTemplateConfigFromOrchestrationObj();

                    OrchestrationAgent orchestrationAgent = new()
                    {
                        Name = agentConfig.Name,
                        Speciality = agentConfig.Description,
                    };

                    orchestrationTemplateConfig.OrchestrationAgents.Add(orchestrationAgent);
                }
            }
        }

        public OrchestrationConfigTemplate? GetOrchestrationConfigFromRegistry(string orchestrationName)
        {

            _orchestrationRegistry.Orchestrations.TryGetValue(orchestrationName, out ChatOrchestration? chatOrchestration);

            if(chatOrchestration is not null)
            {
                return chatOrchestration.GetTemplateConfigFromOrchestrationObj();
            }

            return null;
        }

        public async Task SaveOrchestrationConfigFile(OrchestrationConfigTemplate orchestrationConfigTemplate)
        {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var orchestrationTemplateSubdirectories = templateSection
                .GetSection("Orchestration")
                .GetSection("SubDirectories")
                .Get<string[]>();

            var jsonString = JsonSerializer.Serialize(orchestrationConfigTemplate);
            Console.WriteLine(jsonString);

            var directoryPath = FileHelper.BuildPathFromProjectDirectory(orchestrationTemplateSubdirectories);
            var saveFilePath = FileHelper.BuildFilePath(directoryPath, orchestrationConfigTemplate.Filename);

            await _storageProvider.SaveFileAsync<OrchestrationConfigTemplate>(saveFilePath, orchestrationConfigTemplate);
            Console.WriteLine("Orchestration has been saved, successfully!");
        }
    }
}
