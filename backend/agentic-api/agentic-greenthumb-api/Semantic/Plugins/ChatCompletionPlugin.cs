using AgenticGreenthumbApi.Models;
using AgenticGreenthumbApi.Semantic.Agents;
using AgenticGreenthumbApi.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Text.Json;

namespace AgenticGreenthumbApi.Semantic.Plugins
{
    public class ChatCompletionPlugin
    {

        public static class KernelFunction
        {
            public const string GetProjectInformation = nameof(GetProjectInformation);
            public const string GetHumidityData = nameof(GetHumidityData);
        }

        //NOTE: PROCESS AND ORCESTRATION ALLOWS AGENTS TO COMMUNICATE AND RELAY TO EACH OTHER. WE CANNOT CALL AN AGENT INSIDE A PLUGIN ASSIGNED TO ANOTHER AGENT.

        public ChatCompletionPlugin()
        {
        }

        [KernelFunction(KernelFunction.GetProjectInformation)]
        [Description("Retrieve Information About the IoT Project givent the context of the project.")]
        [return: Description("Return Information about the IoT Gardening Project given the context of the project.")]
        public string GetProjectInformation()
        {
            return "Return Information about the IoT Gardening Project given the context of the project as prompted by the user.";
        }
    }
}
