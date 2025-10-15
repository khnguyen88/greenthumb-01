using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Models;

namespace AgenticGreenthumbApi.Mappers
{
    public class AdafruitFeedMapper<T>
    {
        public static AdafruitFeed<T> ModelToDomainMapper(AdafruitFeedModel<T> model)
        {
            return new AdafruitFeed<T>()
            {
                Value = model.Value,
                CreatedAt = model.CreatedAt
            };
        }
    }
}
