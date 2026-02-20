using AgenticGreenthumbApi.Factory;

namespace AgenticGreenthumbApi.Providers
{
    public class KernelFactoryProvider : IFactoryProvider<KernelFactory>
    {
        private KernelFactory? _kernelFactory;
        private readonly SemaphoreSlim _lock = new(1, 1);
        private readonly IConfiguration _config;
        private readonly IServiceProvider _sp;

        public KernelFactoryProvider(IConfiguration config, IServiceProvider sp)
        {
            _config = config;
            _sp = sp;
        }

        public async Task<KernelFactory> GetFactoryAsync()
        {
            if (_kernelFactory != null) return _kernelFactory;

            await _lock.WaitAsync();
            try
            {
                if (_kernelFactory == null)
                {
                    var kernelFactory = await KernelFactory.CreateAsync(_config, _sp);
                    _kernelFactory = kernelFactory;
                }
            }
            finally
            {
                _lock.Release();
            }

            return _kernelFactory;
        }
    }
}
