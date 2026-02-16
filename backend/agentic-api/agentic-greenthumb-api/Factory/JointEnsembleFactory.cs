using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.JointEnsembles;
using AgenticGreenthumbApi.Semantic.Orchestrations;
using Microsoft.SemanticKernel.Agents.Orchestration;

namespace AgenticGreenthumbApi.Factory
{
    public class JointEnsembleFactory
    {
        private readonly IConfiguration _config;
        private readonly OrchestrationFactory _orchestrationFactory;
        private readonly JointEnsembleRegistry _jointEnsembleRegistry;
        public JointEnsembleFactory(IConfiguration config, OrchestrationFactory orchestrationFactory, JointEnsembleRegistry jointEnsembleRegistry) {
            _config = config;
            _orchestrationFactory = orchestrationFactory;
            _jointEnsembleRegistry = jointEnsembleRegistry;

            Initialize();
        }

        private void Initialize() {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var jointEnsembleTemplateSubdirectories = templateSection.GetSection("JointEnsemble")
                .GetSection("SubDirectories")
                .Get<string[]>();

            List<JointEnsembleConfigTemplate> jointEnsembleConfigTemplates = FileReaderHelper.GetTemplateFiles<JointEnsembleConfigTemplate>(jointEnsembleTemplateSubdirectories);

            foreach (var template in jointEnsembleConfigTemplates)
            {
                try {
                    ChatJointEnsemble jointEnsemble = new SequentialJointEnsemble(template, _orchestrationFactory.GetOrchestrationRegistry().Orchestrations.Values.ToArray());

                    if (jointEnsemble.JointEnsemble.Count > 0) {
                        _jointEnsembleRegistry.JointEnsembles.TryAdd(jointEnsemble.Name, jointEnsemble);
                    }

                }
                catch (Exception ex) {
                    Console.WriteLine($"Attempts to generate join ensembles have falled due this error: {ex.Message}");
                }
            }
        }

        public JointEnsembleRegistry GetOrchestrationRegistry()
        {
            return _jointEnsembleRegistry;
        }
    }


}
