using AgenticGreenthumbApi.Semantic.Orchestrations;
using System.Text.Json.Serialization;

namespace AgenticGreenthumbApi.Domain
{
    public class JointEnsembleConfigTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;

        public List<string> ChatOrchestrationNames { get; set; } = new();
    }
}
