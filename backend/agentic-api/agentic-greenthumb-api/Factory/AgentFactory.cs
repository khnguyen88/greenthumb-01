using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using Elastic.Clients.Elasticsearch;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Runtime;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using NRedisStack;

namespace AgenticGreenthumbApi.Factory
{
    public class AgentFactory
    {
        private readonly IConfiguration _config;
        private readonly KernelFactory _kernelFactory;
        private AgentRegistry _agentRegistry;

        public AgentFactory(IConfiguration config, KernelFactory kernelFactory, AgentRegistry agentRegistry) {
            _config = config;
            _kernelFactory = kernelFactory;
            _agentRegistry = agentRegistry;

            Initialize(agentRegistry);
        }

        public void Initialize(AgentRegistry agentRegistry)
        {
            IConfigurationSection templateSection = _config.GetSection("Template");

            var agentTemplateSubdirectories = templateSection.GetSection("Agent")
                .GetSection("SubDirectories")
                .Get<string[]>();

            List<AgentTemplate> agentTemplates= FileReaderHelper.GetAgentTemplates(agentTemplateSubdirectories);
            agentTemplates.ForEach(template => {
                try
                {
                    //Clone the master kernel
                    Kernel kernel = _kernelFactory.GetNewKernel();


                    //Select specified kernel tools
                    List<KernelFunction> kernelFunctions = new List<KernelFunction>();
                    FunctionChoiceBehavior functionChoiceBehavior = FunctionChoiceBehavior.Auto();

                    var kernelTools = template.KernelTools;

                    if (kernelTools.KernelPlugins.Count > 0 || kernelTools.KernelFunctions.Count > 0)
                    {
                        // Either select specific kernel functions or specific plugins
                        // In terms of kernel functions, we select the ones required and filters the rest out
                        if (kernelTools.UseSpecificFunctionsOnly && kernelTools?.KernelFunctions.Count > 0)
                        {
                            KernelFunction? kernelFunction = null;

                            kernelTools.KernelFunctions.ForEach(f => {
                                kernel.Plugins.TryGetFunction(f.PluginName, f.FunctionName, out kernelFunction);

                                if (kernelFunction != null){
                                    kernelFunctions.Add(kernelFunction);
                                }
                            });


                        }
                        else
                        {

                            //In terms of plugin selection, we remove the ones not specified. There is no option to filter, so we have to manually remove it.
                            Kernel tempKernel = _kernelFactory.GetNewKernel();

                            foreach (var p in tempKernel.Plugins)
                            {
                                if (!kernelTools.KernelPlugins.Contains(p.Name))
                                {
                                    kernel.Plugins.Remove(p);
                                }
                            }
                        }
                    }

                    var kernelArguments = template.KernelArguments;

                    switch (kernelArguments.FunctionChoiceBehavior.ToLower())
                    {
                        case "required":
                            functionChoiceBehavior = FunctionChoiceBehavior.Required(functions: kernelFunctions);
                            break;
                        case "none":
                            functionChoiceBehavior = FunctionChoiceBehavior.None();
                            break;
                        default:
                            functionChoiceBehavior = FunctionChoiceBehavior.Auto();
                            break;
                    }

                    //Build the prompt execution settings

                    var openAIPromptExecutionSettings = new OpenAIPromptExecutionSettings
                    {
                        FunctionChoiceBehavior = functionChoiceBehavior,
                        ServiceId = kernelArguments.ServiceId,
                        ModelId = kernelArguments.ModelId,
                        Temperature = kernelArguments.Temperature,
                        TopP = kernelArguments.TopP,
                        MaxTokens = kernelArguments.MaxTokens,
                        PresencePenalty = kernelArguments.PresencePenalty,
                        FrequencyPenalty = kernelArguments.FrequencyPenalty,
                    };

                    //Build the instruction
                    string instructions = template.Instruction;

                    var contextItems = template.Context;
                    if (contextItems.Count > 0)
                    {
                        string context = string.Empty;
                        instructions = instructions + "\n\n" + $"Here are some additional context for the instruction:";

                        contextItems.ForEach(c => {
                            context = FileReaderHelper.GetFileFromDirectory(c.FileDirectory, c.FileName);
                            instructions = instructions + "\n\n" + c.Detail + ": " + "\n" + context;
                        });
                    }


                    Agent[] agent = [
                        new ChatCompletionAgent
                    {
                        Name = template.Name,
                        Instructions = instructions,
                        Description = template.Description,
                        Kernel = kernel,
                        Arguments = new KernelArguments(openAIPromptExecutionSettings),
                    }
                    ];
                    Console.WriteLine("Before Adding Agents: " + _agentRegistry.Agents.ToList().Count);
                    _agentRegistry.Agents = _agentRegistry.Agents.Concat(agent).ToArray();
                    Console.WriteLine("After Adding Agents: " + _agentRegistry.Agents.ToList().Count);
                    Console.WriteLine("Let's count agents added to the Registry from the factory: " + _agentRegistry.Agents.ToList().Count);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Attempts to generate agents have falled due this error: {ex.Message}");
                }
            });
        }

        public AgentRegistry GetAgentRegistry()
        {
            return _agentRegistry;
        }

    }
}
