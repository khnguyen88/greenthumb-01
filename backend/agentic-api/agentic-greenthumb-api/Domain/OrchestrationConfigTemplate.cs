using System.Text.Json.Serialization;

namespace AgenticGreenthumbApi.Domain
{
    public class OrchestrationConfigTemplate
    {

        public string Name { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrchestrationType Type { get; set; }

        public List<OrchestrationAgent> OrchestrationAgents { get; set; } = new();
        public int MaximumInvocationCount { get; set; } = 1;
        public int InvocationTimeLimitSecs { get; set; } = 180;
    }

    public class OrchestrationAgent
    {
        public string Name { get; set; } = string.Empty;
        public bool IsLead { get; set; } = false;
        public string Speciality { get; set; } = string.Empty ;
    }
    public enum OrchestrationType
    {
        Single,
        Sequential,
        Concurrent,
        Handoff,
        Magnetic
    }
}
