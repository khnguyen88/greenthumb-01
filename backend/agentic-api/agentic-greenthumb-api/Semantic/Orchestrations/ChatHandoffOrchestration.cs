using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Helper;
using AgenticGreenthumbApi.Semantic.Agents;
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
    public class ChatHandoffOrchestration
    {
        public HandoffOrchestration HandoffOrchestration { get; set; }
        public ChatHistory ChatHistory { get; set; } = [];

        public ChatHandoffOrchestration(OrchestrationConfig orchestrationConfig, params Agent[] agents)
        {
            //Kernel
            Kernel managerKernel = KernelFactoryHelper.GetNewKernel();

            //Chat History
            ChatHistory = [];

            ValueTask ResponseCallback(ChatMessageContent response)
            {
                Console.WriteLine();
                Console.WriteLine($"# {response.Role} - {response.AuthorName}: {response.Content}");
                Console.WriteLine();
                ChatHistory.Add(response);

                return ValueTask.CompletedTask;
            }

            //Agents
            Agent[] orchestrationAgents = agents.Where(a => orchestrationConfig.OrchestrationAgents.Any(oa => oa.Name == a.Name)).ToArray();
            Agent orchestrationLeadAgent = orchestrationAgents.FirstOrDefault(a => orchestrationConfig.OrchestrationAgents.Any(oa => oa.IsLead == true && a.Name == oa.Name));
            Agent[] orchestrationWorkerAgents = orchestrationAgents.Where(a => orchestrationConfig.OrchestrationAgents.Any(oa => oa.IsLead == false && a.Name != orchestrationLeadAgent.Name)).ToArray();


            //Handoff Setup (TODO, SET UP AN ORCHESTRATION TEMPLATE 
            OrchestrationHandoffs handoffs = OrchestrationHandoffs
                .StartWith(orchestrationLeadAgent)
                .Add(orchestrationLeadAgent, orchestrationWorkerAgents);


            foreach (var workerAgent in orchestrationWorkerAgents)
            {
                var agentConfigInfo = orchestrationConfig.OrchestrationAgents.FirstOrDefault(oa => oa.Name == workerAgent.Name);
                var agentConfigDescription = (bool)(agentConfigInfo.Speciality.IsNullOrEmpty()) ? workerAgent.Description : agentConfigInfo?.Speciality.ToString();
                handoffs.Add(workerAgent, orchestrationLeadAgent, $"Transfer to {orchestrationLeadAgent.Name.ToLower()} if the issue is not {workerAgent.Name.ToLower()} related. Specifically if the issue is not related to {agentConfigDescription}.");
            }

            //Handoff Orchestration
            HandoffOrchestration = new HandoffOrchestration(handoffs, agents)
            {
                ResponseCallback = ResponseCallback,
            };
        }

        public void ClearChatHistory()
        {
            ChatHistory.Clear();
        }

        public void SetChatHistory(ChatHistory userChatHistory)
        {
            ChatHistory = userChatHistory;
        }

        public string OutputAssistentResponseContent()
        {
            string combinedResponse = "";

            foreach (var assistantContent in ChatHistory)
            {
                combinedResponse = combinedResponse + $"#{assistantContent.Content}" + "\n\n";
            }

            return combinedResponse;
        }

        public async Task<string> GetResponse(string userPrompt)
        {
            InProcessRuntime runtime = new InProcessRuntime();

            await runtime.StartAsync();

            OrchestrationResult<string> result = await HandoffOrchestration.InvokeAsync(userPrompt, runtime);
            string output = await result.GetValueAsync(TimeSpan.FromSeconds(180)); //Very important settings
            Console.WriteLine("//----------------//");
            ChatHistory.AddAssistantMessage(output);
            Console.WriteLine(output);

            await runtime.RunUntilIdleAsync();

            return output;
        }
    }
}
#pragma warning restore
