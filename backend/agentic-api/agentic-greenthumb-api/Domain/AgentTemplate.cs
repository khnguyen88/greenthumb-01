namespace AgenticGreenthumbApi.Domain
{
    public class AgentTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string Instruction { get; set; } = string.Empty;
        public ContextDetails ContextDetails { get; set; } = new();
        public string Description { get; set; } = string.Empty;
        public KernelTools KernelTools { get; set; } = new();
        public KernelArguments KernelArguments { get; set; } = new();
    }

    public class ContextDetails
    {
        public bool UseContextFileOnly { get; set; }
        public string ContextFileDirectory { get; set; } = string.Empty;
        public string ContextFileName { get; set; } = string.Empty;
        public string ContextText { get; set; } = string.Empty;
    }

    public class KernelTools
    {
        public bool UseSpecificFunctionsOnly { get; set; }
        public List<string> KernelPlugins { get; set; } = new();
        public List<KernelFunctionRef> KernelFunctions { get; set; } = new();
    }

    public class KernelFunctionRef
    {
        public string PluginName { get; set; } = string.Empty;
        public string FunctionName { get; set; } = string.Empty;
    }

    public class KernelArguments
    {
        public string ServiceId { get; set; } = string.Empty;
        public string ModelId { get; set; } = string.Empty;
        public string FunctionChoiceBehavior { get; set; } = "Auto";
        public double? Temperature { get; set; } = null;
        public double? TopP { get; set; } = null;
        public int? MaxTokens { get; set; } = null;
        public double? PresencePenalty { get; set; } = null;
        public double? FrequencyPenalty { get; set; } = null;
    }
}
