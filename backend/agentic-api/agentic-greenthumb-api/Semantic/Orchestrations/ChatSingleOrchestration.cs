using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.Sequential;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;

#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatSingleOrchestration: ChatOrchestration
    {
        public SequentialOrchestration SingleOrchestration { get; set; }

        public ChatSingleOrchestration(OrchestrationConfigTemplate orchestrationConfig, params Agent[] agents)
        {
            //Orchestration Config
            OrchestrationConfig = orchestrationConfig;

            //Chat History
            ChatHistory = [];

            //Agents
            Agent[] orchestrationAgents = agents.Where(a => OrchestrationConfig.OrchestrationAgents.Any(oa => oa.Name == a.Name)).ToArray();

            //Single Orchestration
            SingleOrchestration = new SequentialOrchestration(orchestrationAgents.FirstOrDefault())
            {
                ResponseCallback = ResponseCallback,
            };
        }

        public override async Task<string> GetResponse(string userPrompt)
        {
            InProcessRuntime runtime = new InProcessRuntime();

            await runtime.StartAsync();

            OrchestrationResult<string> result = await SingleOrchestration.InvokeAsync(userPrompt, runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(OrchestrationConfig.InvocationTimeLimitSecs)); //Very important settings

            AppendChatHistory(output);

            Console.WriteLine("//----------------//");
            Console.WriteLine(output);

            await runtime.RunUntilIdleAsync();

            return output;
        }
    }
}
