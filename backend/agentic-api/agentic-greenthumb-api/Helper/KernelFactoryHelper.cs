using AgenticGreenthumbApi.Semantic.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using System.Net;


namespace AgenticGreenthumbApi.Helper
{
    public class KernelFactoryHelper
    {
        public static Kernel masterKernel;
        public static IKernelMemory masterKernelMemory;
        private static IConfiguration _config;

        public static void Initialize(IConfiguration config)
        {
            _config = config;


            IConfigurationSection azureFoundarySection = config.GetSection("AzureAIFoundry");

            Dictionary<string, string> ReadSection(IConfigurationSection configurationSection, string name)
            {
                return configurationSection.GetSection(name)
                    .GetChildren()
                    .ToDictionary(c => c.Key, c => c.Value!);
            }

            var model = ReadSection(azureFoundarySection, "AIModel");
            var miniModel = ReadSection(azureFoundarySection, "AIModelMini");
            var embedding = ReadSection(azureFoundarySection, "AIEmbedding");
            var search = ReadSection(azureFoundarySection, "AISearch");
            var blob = ReadSection(azureFoundarySection, "AzureBlobStorage");

            // Configuration Files
            AzureOpenAIConfig textAzureOpenAIConfig = new()
            {
                APIKey = model["ApiKey"],
                Endpoint = model["Uri"],
                Tokenizer = "o200k",
                Deployment = model["Name"],
                APIType = AzureOpenAIConfig.APITypes.TextCompletion,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey

            };

            //AzureOpenAIConfig textAzureOpenAIMiniConfig = new()
            //{
            //    APIKey = miniModel["ApiKey"],
            //    Endpoint = miniModel["Uri"],
            //    Tokenizer = "o200k",
            //    Deployment = miniModel["Name"],
            //    APIType = AzureOpenAIConfig.APITypes.TextCompletion,
            //    Auth = AzureOpenAIConfig.AuthTypes.APIKey

            //};
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
                Auth = AzureBlobsConfig.AuthTypes.AccountKey,
            };


            // Create a kernel with Azure OpenAI chat completion
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();

            kernelBuilder.AddAzureOpenAIChatCompletion(textAzureOpenAIConfig.Deployment, textAzureOpenAIConfig.Endpoint, textAzureOpenAIConfig.APIKey);

            masterKernel = kernelBuilder.Build();

            IKernelMemoryBuilder memoryBuilder = new KernelMemoryBuilder()
                .WithAzureOpenAITextGeneration(textAzureOpenAIConfig)
                .WithAzureOpenAITextEmbeddingGeneration(embeddingAzureOpenAIConfig)
                .WithAzureAISearchMemoryDb(azureAISearchConfig)
                .WithAzureBlobsDocumentStorage(azureBlobsConfig);

            masterKernelMemory = memoryBuilder.Build();
        }


        public static Kernel GetNewKernel()
        {
            return masterKernel.Clone();
        }

        public static IKernelMemory GetNewKernelMemory()
        {
            return FastDeepCloner.DeepCloner.Clone(masterKernelMemory);
        }

        public static Kernel GetMasterKernel()
        {
            return masterKernel;
        }

        public static IKernelMemory GetMasterKernelMemory()
        {
            return masterKernelMemory;
        }
    }
}