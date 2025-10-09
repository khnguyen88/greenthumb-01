using System.Text.Json.Serialization;

namespace AgenticGreenthumbApi.Models
{
    public class AdafruitFeedModel<T>
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("value")]
        public T Value { get; set; }

        [JsonPropertyName("feed_id")]
        public int FeedId { get; set; }

        [JsonPropertyName("feed_key")]
        public string FeedKey { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("created_epoch")]
        public long CreatedEpoch { get; set; }

        [JsonPropertyName("expiration")]
        public DateTime Expiration { get; set; }

    }
}
