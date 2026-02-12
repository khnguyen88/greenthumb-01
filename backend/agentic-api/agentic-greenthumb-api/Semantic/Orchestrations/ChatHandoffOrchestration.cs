using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.ML.OnnxRuntimeGenAI;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.Agents.Magentic;
using Microsoft.SemanticKernel.Agents.Orchestration;
using Microsoft.SemanticKernel.Agents.Orchestration.Handoff;
using Microsoft.SemanticKernel.Agents.Runtime.InProcess;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System.Text.Json;


#pragma warning disable
namespace AgenticGreenthumbApi.Semantic.Orchestrations
{
    public class ChatHandoffOrchestration: ChatOrchestration
    {
        public HandoffOrchestration HandoffOrchestration { get; set; }

        public override string Type { get; set; } = OrchestrationType.Handoff.ToString();

        public ChatHandoffOrchestration(OrchestrationConfigTemplate orchestrationConfig, params Agent[] agents)
        {
            //Orchestration Config
            OrchestrationConfig = orchestrationConfig;

            //Chat History
            ChatHistory = [];

            //Agents
            Agent[] orchestrationAgents = agents.Where(a => OrchestrationConfig.OrchestrationAgents.Any(oa => oa.Name == a.Name)).ToArray();
            Agent orchestrationLeadAgent = orchestrationAgents.FirstOrDefault(a => OrchestrationConfig.OrchestrationAgents.Any(oa => oa.IsLead == true && a.Name == oa.Name));
            Agent[] orchestrationWorkerAgents = orchestrationAgents.Where(a => OrchestrationConfig.OrchestrationAgents.Any(oa => oa.IsLead == false && a.Name != orchestrationLeadAgent.Name)).ToArray();


            //Handoff Setup
            OrchestrationHandoffs handoffs = OrchestrationHandoffs
                .StartWith(orchestrationLeadAgent)
                .Add(orchestrationLeadAgent, orchestrationWorkerAgents);


            foreach (var workerAgent in orchestrationWorkerAgents)
            {
                var agentConfigInfo = OrchestrationConfig.OrchestrationAgents.FirstOrDefault(oa => oa.Name == workerAgent.Name);
                var agentConfigDescription = (bool)(agentConfigInfo.Speciality.IsNullOrEmpty()) ? workerAgent.Description : agentConfigInfo?.Speciality.ToString();
                handoffs.Add(workerAgent, orchestrationLeadAgent, $"Transfer to {orchestrationLeadAgent.Name.ToLower()} if the issue is not {workerAgent.Name.ToLower()} related. Specifically if the issue is not related to {agentConfigDescription}.");
            }

            //Handoff Orchestration
            HandoffOrchestration = new HandoffOrchestration(handoffs, agents)
            {
                ResponseCallback = ResponseCallback,
            };
        }

        public override async Task<string> GetResponse(string userPrompt)
        {
            InProcessRuntime runtime = new InProcessRuntime();

            await runtime.StartAsync();

            OrchestrationResult<string> result = await HandoffOrchestration.InvokeAsync(userPrompt, runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(OrchestrationConfig.InvocationTimeLimitSecs)); //Very important settings

            AppendChatHistory(output);

            Console.WriteLine("//----------------//");
            Console.WriteLine(output);

            await runtime.RunUntilIdleAsync();

            return output;
        }
    }
}
#pragma warning restore
