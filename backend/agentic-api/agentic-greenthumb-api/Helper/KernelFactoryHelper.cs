using Microsoft.Extensions.Configuration;
using Microsoft.KernelMemory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Handlebars;
using Microsoft.Extensions.Http.Resilience;
using System.Net;


namespace AgenticGreenthumbApi.Helper
{
    public class KernelFactoryHelper
    {
        public static Kernel masterKernel;
        public static IKernelMemory masterKernelMemory;

        static KernelFactoryHelper() {

            // var httpClient = new HttpClient
            // {
            //     Timeout = TimeSpan.FromMinutes(10),
            // };

            var config = new ConfigurationBuilder().AddUserSecrets("4f91f0a7-edfa-4d74-b7d8-6f7a324e86fb").Build();

            // Configuration Files Values
            string textDeploymentName = config["AzureAIFoundry:AIModel:Name"]!;
            string endpoint = config["AzureAIFoundry:AIModel:Uri"]!;
            string apiKey = config["AzureAIFoundry:AIModel:ApiKey"]!;

            string embeddingDeploymentName = config["AzureAIFoundry:AIEmbedding:Name"]!;
            string embeddingEndpoint = config["AzureAIFoundry:AIEmbedding:Uri"]!;
            string embeddingApiKey = config["AzureAIFoundry:AIEmbedding:ApiKey"]!;

            string searchEndpoint = config["AzureAIFoundry:AISearch:Uri"]!;
            string searchApiKey = config["AzureAIFoundry:AISearch:ApiKey"]!;

            string blobStorageAccount = config["AzureAIFoundry:AzureBlobStorage:AccountName"]!;
            string blobStorageContainerName = config["AzureAIFoundry:AzureBlobStorage:ContainerName"]!;
            string blobStorageEndpoint = config["AzureAIFoundry:AzureBlobStorage:Uri"]!;
            string blobStorageKey = config["AzureAIFoundry:AzureBlobStorage:ApiKey"]!;

            // Configuration Files
            AzureOpenAIConfig textAzureOpenAIConfig = new()
            {
                APIKey = apiKey,
                Endpoint = endpoint,
                Tokenizer = "o200k",
                Deployment = textDeploymentName,
                APIType = AzureOpenAIConfig.APITypes.TextCompletion,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey

            };
            AzureOpenAIConfig embeddingAzureOpenAIConfig = new()
            {
                APIKey = apiKey,
                Endpoint = endpoint,
                Tokenizer = "o200k",
                Deployment = embeddingDeploymentName,
                APIType = AzureOpenAIConfig.APITypes.EmbeddingGeneration,
                Auth = AzureOpenAIConfig.AuthTypes.APIKey
            };

            AzureAISearchConfig azureAISearchConfig = new()
            {
                APIKey = searchApiKey,
                Endpoint = searchEndpoint,
                Auth = AzureAISearchConfig.AuthTypes.APIKey
            };

            AzureBlobsConfig azureBlobsConfig = new()
            {
                Account = blobStorageAccount,
                AccountKey = blobStorageKey,
                Container = blobStorageContainerName,
                Auth = AzureBlobsConfig.AuthTypes.AccountKey,
            };


            // Create a kernel with Azure OpenAI chat completion
            IKernelBuilder kernelBuilder = Kernel.CreateBuilder();



            // Note if needed: adjust timeout configuration, code obtained here: https://github.com/microsoft/semantic-kernel/discussions/3412
 

            kernelBuilder.AddAzureOpenAIChatCompletion(textDeploymentName, endpoint, apiKey);

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
