namespace AgenticGreenthumbApi.Providers
{
    public interface IStorageProvider
    {
        Task<string?> GetFileAsync(string path, CancellationToken cancellationToken = default);
        Task<string[]?> GetFilesAsync(string path, CancellationToken cancellationToken = default);

        Task SaveFileAsync<T>(string path, T content, CancellationToken cancellationToken = default);

    }
}
