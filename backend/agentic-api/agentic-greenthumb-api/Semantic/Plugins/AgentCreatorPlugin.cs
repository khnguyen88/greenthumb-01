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
        private readonly IStorageProvider _storageProvider;

        public static class AgentCreatorFunctions
        {
            public const string SaveAgentAsync = nameof(SaveAgentAsync);
        }

        public AgentCreatorPlugin(IConfiguration config, LocalStorageProvider localStorageProvider)
        {
            _config = config;
            _storageProvider = localStorageProvider;
        }

        [KernelFunction(AgentCreatorFunctions.SaveAgentAsync)]
        [Description("Saves the generated agent to a designated storage location. Ensure that the 'Name' property is PascalCase. Ensure that the 'Filename' field is kebab-case and has the '.json' file extension at the end.")]
        public async Task<string> SaveAgentAsync(AgentConfigTemplate agentConfigTemplate)
        {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var agentTemplateSubdirectories = templateSection
                .GetSection("Agent")
                .GetSection("SubDirectories")
                .Get<string[]>();

            agentConfigTemplate.KernelArguments = new KernelArgumentDetails();

            var jsonString = JsonSerializer.Serialize(agentConfigTemplate);
            Console.WriteLine(jsonString);

            var directoryPath = FileHelper.BuildPathFromProjectDirectory(agentTemplateSubdirectories);
            var saveFilePath = FileHelper.BuildFilePath(directoryPath, agentConfigTemplate.Filename);

            await _storageProvider.SaveFileAsync<AgentConfigTemplate>(saveFilePath, agentConfigTemplate);

            return "Agent saved successfully";
        }
    }
}
