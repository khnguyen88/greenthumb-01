namespace AgenticGreenthumbApi.Providers
{
    public interface IFactoryProvider<T>
    {
        Task<T> GetFactoryAsync();
    }
}
