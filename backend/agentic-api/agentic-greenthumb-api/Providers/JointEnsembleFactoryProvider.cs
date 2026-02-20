using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Factory;
using Microsoft.ML.OnnxRuntimeGenAI;
using Microsoft.SemanticKernel.Agents;

namespace AgenticGreenthumbApi.Providers
{
    public class JointEnsembleFactoryProvider: IFactoryProvider<JointEnsembleFactory>
    {
        private readonly IConfiguration _config;
        private JointEnsembleFactory? _jointEnsembleFactory;
        private JointEnsembleRegistry _jointEnsembleRegistry;
        private readonly IFactoryProvider<OrchestrationFactory>? _orchestrationFactoryProvider;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public JointEnsembleFactoryProvider(IConfiguration config, IFactoryProvider<OrchestrationFactory> orchestrationFactoryProvider, JointEnsembleRegistry jointEnsembleRegistry)
        {
            _config = config;
            _orchestrationFactoryProvider = orchestrationFactoryProvider;
            _jointEnsembleRegistry = jointEnsembleRegistry;
        }

        public async Task<JointEnsembleFactory> GetFactoryAsync()
        {
            var orchestrationFactory = await _orchestrationFactoryProvider.GetFactoryAsync();

            if (_jointEnsembleFactory != null)
            {
                return _jointEnsembleFactory;
            }
            await _lock.WaitAsync();
            try
            {
                if (_jointEnsembleFactory == null)
                {
                    _jointEnsembleFactory = await JointEnsembleFactory.CreateAsync(_config, orchestrationFactory, _jointEnsembleRegistry);
                }
            }
            finally
            {
                _lock.Release();
            }

            return _jointEnsembleFactory;
        }
    }
}
