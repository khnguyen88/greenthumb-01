using System.Text.Json.Serialization;

namespace AgenticGreenthumbApi.Domain
{
    public class AdafruitFeed<T>
    {
        public T Value { get; set; }
        public string Unit { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
