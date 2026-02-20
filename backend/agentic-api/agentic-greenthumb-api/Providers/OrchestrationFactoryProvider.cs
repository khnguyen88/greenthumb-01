using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Factory;
using Microsoft.ML.OnnxRuntimeGenAI;
using Microsoft.SemanticKernel.Agents;

namespace AgenticGreenthumbApi.Providers
{
    public class OrchestrationFactoryProvider : IFactoryProvider<OrchestrationFactory>
    {
        private readonly IConfiguration _config;
        private readonly IFactoryProvider<KernelFactory> _kernelFactoryProvider;
        private readonly IFactoryProvider<Factory.AgentFactory> _agentFactoryProvider;
        private readonly OrchestrationRegistry _orchestrationRegistry;

        private OrchestrationFactory? _orchestrationFactory;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public OrchestrationFactoryProvider(
            IConfiguration config,
            IFactoryProvider<KernelFactory> kernelFactoryProvider,
            IFactoryProvider<Factory.AgentFactory> agentFactoryProvider,
            OrchestrationRegistry orchestrationRegistry)
        {
            _config = config;
            _kernelFactoryProvider = kernelFactoryProvider;
            _agentFactoryProvider = agentFactoryProvider;
            _orchestrationRegistry = orchestrationRegistry;
        }

        public async Task<OrchestrationFactory> GetFactoryAsync()
        {
            var kernelFactory = await _kernelFactoryProvider.GetFactoryAsync();
            var agentFactory = await _agentFactoryProvider.GetFactoryAsync();

            if (_orchestrationFactory != null) return _orchestrationFactory;

            await _lock.WaitAsync();
            try
            {
                if (_orchestrationFactory == null)
                {
                    _orchestrationFactory = await OrchestrationFactory.CreateAsync(
                        _config,
                        kernelFactory,
                        agentFactory,
                        _orchestrationRegistry);
                }
            }
            finally
            {
                _lock.Release();
            }

            return _orchestrationFactory;
        }
    }
}
