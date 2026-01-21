namespace AgenticGreenthumbApi.Domain
{
    public class OrchestrationConfig
    {
        public string Type { get; set; } = string.Empty;
        public List<OrchestrationAgent> OrchestrationAgents { get; set; } = new();
        public int MaximumInvocationCount { get; set; } = 1;
    }

    public class OrchestrationAgent
    {
        public string Name { get; set; } = string.Empty;
        public bool IsLead { get; set; } = false;
        public string Speciality { get; set; } = string.Empty ;
    }


}
