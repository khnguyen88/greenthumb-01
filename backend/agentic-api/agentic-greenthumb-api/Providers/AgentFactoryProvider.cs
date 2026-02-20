using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Factory;

namespace AgenticGreenthumbApi.Providers
{
    public class AgentFactoryProvider: IFactoryProvider<AgentFactory>
    {
        private AgentFactory? _agentFactory;
        private readonly IConfiguration _config;
        private readonly IFactoryProvider<KernelFactory> _kernelFactoryProvider;
        private readonly AgentRegistry _agentRegistry;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public AgentFactoryProvider(IConfiguration config, IFactoryProvider<KernelFactory> kernelFactoryProvider, AgentRegistry agentRegistry)
        {
            _config = config;
            _kernelFactoryProvider = kernelFactoryProvider;
            _agentRegistry = agentRegistry;
        }

        public async Task<AgentFactory> GetFactoryAsync()
        {
            var kernelFactory = await _kernelFactoryProvider.GetFactoryAsync();

            if (_agentFactory != null) {
                return _agentFactory;
            }
            await _lock.WaitAsync();
            try
            {
                if(_agentFactory == null)
                {
                    _agentFactory = await AgentFactory.CreateAsync(_config, kernelFactory, _agentRegistry);
                }
            }
            finally
            {
                _lock.Release();
            }

            return _agentFactory;

        }
    }
}
