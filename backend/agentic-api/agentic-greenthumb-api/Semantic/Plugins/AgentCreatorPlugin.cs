using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Providers;
using AgenticGreenthumbApi.Services;
using Microsoft.ML.OnnxRuntimeGenAI;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text.Json;

namespace AgenticGreenthumbApi.Semantic.Plugins
{
    public class AgentCreatorPlugin
    {
        private readonly IConfiguration _config;
        private readonly SemanticKernelService _semanticKernelService;

        public static class AgentCreatorFunctions
        {
            public const string SaveAgentAsync = nameof(SaveAgentAsync);
            public const string AddAgentToExistingOrchestrationAsync = nameof(AddAgentToExistingOrchestrationAsync);
        }

        public AgentCreatorPlugin(IConfiguration config, SemanticKernelService semanticKernelService)
        {
            _config = config;
            _semanticKernelService = semanticKernelService;
        }

        [KernelFunction(AgentCreatorFunctions.SaveAgentAsync)]
        [Description("Saves the generated agent to a designated storage location. Ensure that the 'Name' property is PascalCase and include the word 'Agent' at the end. Ensure that the 'Filename' field is kebab-case and has the '.json' file extension at the end.")]
        public async Task<string> SaveAgentAsync(AgentConfigTemplate agentConfigTemplate)
        {
            agentConfigTemplate.KernelArguments = new KernelArgumentDetails();
            try
            {
                await _semanticKernelService.UpdateAgentRegistry(agentConfigTemplate);

                await _semanticKernelService.SaveAgentConfigFile(agentConfigTemplate);

                return "Agent saved successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return $"Agent failed to save. Due to error: {ex.ToString()}";
            }
        }

        [KernelFunction(AgentCreatorFunctions.AddAgentToExistingOrchestrationAsync)]
        [Description("Update an existing orchestration and adds a new agent to it. Call this function if user specifies adding to an orchestration and provides a name in PascalCase. If the user does not specify an orchestration name, do not execute function. Ensure that the 'Name' property is PascalCase and include the word 'Agent' at the end. Ensure that the 'Filename' field is kebab-case and has the '.json' file extension at the end. Orchestration name as user specified, but must be in PascalCase.")]
        public async Task<string> AddAgentToExistingOrchestrationAsync(AgentConfigTemplate agentConfigTemplate, string orchestrationName)
        {
            agentConfigTemplate.KernelArguments = new KernelArgumentDetails();
            try
            {
                OrchestrationConfigTemplate? orchestrationConfigTemplate = _semanticKernelService.GetOrchestrationConfigFromRegistry(orchestrationName);

                if(orchestrationConfigTemplate is not null)
                {
                    await _semanticKernelService.UpdateOrchestrationRegistryAddAgents(agentConfigTemplate, orchestrationConfigTemplate.Name);

                    await _semanticKernelService.SaveOrchestrationConfigFile(orchestrationConfigTemplate);

                    return $"Agent has been successfully added to the {orchestrationName} in the registry and in the config file, successfully";
                }

                return $"Agent was not successfully added to {orchestrationName}. Either {agentConfigTemplate.Name} or {orchestrationName} does not exist in the registries. Please try again.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return $"Process to add agent to an existing orchestation has failed, due to error: {ex.ToString()}";
            }
        }
    }
}
