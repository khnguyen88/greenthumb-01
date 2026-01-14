using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;

namespace AgenticGreenthumbApi.Factory
{
    public class KernelFactory
    {
        private readonly IConfiguration _config;

        private Kernel _masterKernel;
        private IKernelMemory _masterKernelMemory;
        private readonly IServiceProvider _sp;


        public KernelFactory(IConfiguration config, IServiceProvider sp)
        {
            _config = config;
            _sp = sp;

            Initialize();

            Console.WriteLine("Hi");
        }

        private void Initialize()
        {
            IConfigurationSection azureFoundarySection = _config.GetSection("AzureAIFoundry");

            Dictionary<string, string> ReadSection(IConfigurationSection section, string name)
            {
                return section.GetSection(name)
                    .GetChildren()
                    .ToDictionary(c => c.Key, c => c.Value!);
            }

            var model = ReadSection(azureFoundarySection, "AIModel");
            var embedding = ReadSection(azureFoundarySection, "AIEmbedding");
            var search = ReadSection(azureFoundarySection, "AISearch");
            var blob = ReadSection(azureFoundarySection, "AzureBlobStorage");

            // Azure OpenAI Configurations
            AzureOpenAIConfig textAzureOpenAIConfig = new()
            {
                APIKey = model["ApiKey"],
                Endpoint = model["Uri"],
                Tokenizer = "o200k",
                Deployment = model["Name"],
                APIType = AzureOpenAIConfig.APITypes.TextCompletion,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            AzureOpenAIConfig embeddingAzureOpenAIConfig = new()
            {
                APIKey = embedding["ApiKey"],
                Endpoint = embedding["Uri"],
                Tokenizer = "o200k",
                Deployment = embedding["Name"],
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            AzureAISearchConfig azureAISearchConfig = new()
            {
                APIKey = search["ApiKey"],
                Endpoint = search["Uri"],
                Auth = AzureAISearchConfig.AuthTypes.APIKey
            };

            AzureBlobsConfig azureBlobsConfig = new()
            {
                Account = blob["AccountName"],
                AccountKey = blob["ApiKey"],
                Container = blob["ContainerName"],
                Auth = AzureBlobsConfig.AuthTypes.AccountKey
            };

            // Build Kernel
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.AddAzureOpenAIChatCompletion(
                textAzureOpenAIConfig.Deployment,
                textAzureOpenAIConfig.Endpoint,
                textAzureOpenAIConfig.APIKey
            );

            _masterKernel = kernelBuilder.Build();

            AddAllPluginsFromNamespace(_masterKernel, _sp);

            // Build Memory
            IKernelMemoryBuilder memoryBuilder = new KernelMemoryBuilder()
                .WithAzureOpenAITextGeneration(textAzureOpenAIConfig)
                .WithAzureOpenAITextEmbeddingGeneration(embeddingAzureOpenAIConfig)
                .WithAzureAISearchMemoryDb(azureAISearchConfig)
                .WithAzureBlobsDocumentStorage(azureBlobsConfig);

            _masterKernelMemory = memoryBuilder.Build();
        }

        public Kernel GetNewKernel()
        {
            return _masterKernel.Clone();
        }

        public IKernelMemory GetNewKernelMemory()
        {
            return FastDeepCloner.DeepCloner.Clone(_masterKernelMemory);
        }

        public Kernel GetMasterKernel() => _masterKernel;

        public IKernelMemory GetMasterKernelMemory() => _masterKernelMemory;

        public static void AddAllPluginsFromNamespace(Kernel kernel, IServiceProvider sp)
        {
            var pluginTypes = typeof(Program).Assembly
                .GetTypes()
                .Where(t =>
                    t.IsClass &&
                    !t.IsAbstract &&
                    t.Namespace == "AgenticGreenthumbApi.Semantic.Plugins");

            Console.WriteLine("hi");
            Console.WriteLine(pluginTypes.ToList().Count);

            foreach (var type in pluginTypes)
            {
                var instance = sp.GetRequiredService(type);
                kernel.Plugins.AddFromObject(instance, type.Name);
                Console.WriteLine(type.Name);
            }
        }
    }
}
