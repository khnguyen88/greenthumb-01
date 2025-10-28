using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
using Microsoft.SemanticKernel;

namespace AgenticGreenthumbApi.Mappers
{
    public class ChatHistoryMapper
    {
        public static List<ChatMessageDto> DomainToDtoMapper(List<ChatMessageContent> chatHistory)
        {
            return chatHistory.Select(x =>
                new ChatMessageDto()
                {
                    Role = x.Role.Label.ToLowerInvariant(),
                    Message = x.Items[0].ToString()
                }).ToList();
        }
    }
}
