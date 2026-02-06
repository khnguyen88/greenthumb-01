using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.Concurrent;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Text;

#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatConcurrentOrchestration: ChatOrchestration
    {
        public ConcurrentOrchestration ConcurrentOrchestration { get; set; }

        public ChatConcurrentOrchestration(OrchestrationConfigTemplate orchestrationConfig, params Agent[] agents)
        {
            //Orchestration Config
            OrchestrationConfig = orchestrationConfig;

            //Chat History
            ChatHistory = [];

            //Agents
            Agent[] orchestrationAgents = agents.Where(a => OrchestrationConfig.OrchestrationAgents.Any(oa => oa.Name == a.Name)).ToArray();

            //Concurrent Orchestration
            ConcurrentOrchestration = new ConcurrentOrchestration(orchestrationAgents)
            {
                ResponseCallback = ResponseCallback,
            };
        }

        public override async Task<string> GetResponse(string userPrompt)
        {
            InProcessRuntime runtime = new InProcessRuntime();

            await runtime.StartAsync();

            OrchestrationResult<string[]> results = await ConcurrentOrchestration.InvokeAsync(userPrompt, runtime);
            string[] outputs = await results.GetValueAsync(TimeSpan.FromSeconds(OrchestrationConfig.InvocationTimeLimitSecs)); //Very important settings

            StringBuilder output = new StringBuilder();

            foreach (var item in outputs)
            {
                output.AppendLine(item);
                output.AppendLine("\n\n");
            }
            AppendChatHistory(output.ToString());

            Console.WriteLine("//----------------//");
            Console.WriteLine(output.ToString());

            await runtime.RunUntilIdleAsync();

            return output.ToString();
        }
    }
}
#pragma warning enable