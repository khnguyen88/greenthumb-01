namespace AgenticGreenthumbApi.Services
{
    public class AgenticMemoryService
    {
        private readonly ILogger<AgenticMemoryService> _logger;

        public AgenticMemoryService(ILogger<AgenticMemoryService> logger)
        {
            _logger = logger;
        }
    }
}
