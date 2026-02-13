using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Azure;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;

#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatMagenticOrchestration: ChatOrchestration
    {
        public MagenticOrchestration MagenticOrchestration { get; set; }

        public override string Type { get; set; } = OrchestrationType.Magnetic.ToString();

        public ChatMagenticOrchestration(OrchestrationConfigTemplate orchestrationConfig, Kernel kernel, params Agent[] agents)
        {
            //Name
            Name = orchestrationConfig.Name;

            //Orchestration Config
            OrchestrationConfig = orchestrationConfig;

            //Chat History
            ChatHistory = [];

            //Agents
            Agent[] orchestrationAgents = agents.Where(a => OrchestrationConfig.OrchestrationAgents.Any(oa => oa.Name == a.Name)).ToArray();

            //Manager
            StandardMagenticManager manager = new StandardMagenticManager(
                kernel.GetRequiredService<IChatCompletionService>(),
                new OpenAIPromptExecutionSettings())
            {
                MaximumInvocationCount = 2, //Very important settings
            };

            //Orchestration
            // =====================================================================================
            MagenticOrchestration = new MagenticOrchestration(manager, orchestrationAgents)
            {
                ResponseCallback = ResponseCallback,
            };
        }

        public override async Task<string> GetResponse(string userPrompt)
        {
            InProcessRuntime runtime = new InProcessRuntime();

            await runtime.StartAsync();

            OrchestrationResult<string> result = await MagenticOrchestration.InvokeAsync(userPrompt, runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(OrchestrationConfig.InvocationTimeLimitSecs)); //Very important settings

            ChatHistoryHelper.AppendChatResponseMessage(ChatHistory, output);


            Console.WriteLine("//----------------//");
            Console.WriteLine(output);

            await runtime.RunUntilIdleAsync();

            return output;
        }
    }
}
#pragma warning enable
