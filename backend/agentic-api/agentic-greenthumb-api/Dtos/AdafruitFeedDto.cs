using System.Text.Json.Serialization;

namespace AgenticGreenthumbApi.Dtos
{
    public class AdafruitFeedDto<T>
    {
        public T Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string CreatedAt { get; set; }
    }
}
