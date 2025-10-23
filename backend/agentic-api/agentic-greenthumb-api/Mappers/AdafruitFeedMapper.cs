using AgenticGreenthumbApi.Domain;
using AgenticGreenthumbApi.Dtos;
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

        public static AdafruitFeedDto<T> DomainToDtoMapper(AdafruitFeed<T> domain)
        {
            return new AdafruitFeedDto<T>()
            {
                Value = domain.Value,
                Unit = domain.Unit,
                CreatedAt = domain.CreatedAt.ToString("yy-MM-dd HH:mm")
            };
        }
    }
}
