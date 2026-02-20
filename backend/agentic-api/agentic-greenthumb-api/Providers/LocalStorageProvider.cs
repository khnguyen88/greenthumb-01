using AgenticGreenthumbApi.Helper;
using System.Text.Json;

namespace AgenticGreenthumbApi.Providers
{
    public class LocalStorageProvider: IStorageProvider
    {
        public async Task<string?> GetFileAsync(string path, CancellationToken cancellationToken = default) {
            return await FileHelper.GetFileFromPathAsync(path);
        }

        public async Task<string[]?> GetFilesAsync(string path, CancellationToken cancellationToken = default) {
            return await Task.FromResult<string[]>([]);
        }

        public async Task SaveFileAsync<T>(string path, T content, CancellationToken cancellationToken = default) {
            string serializedContent = JsonSerializer.Serialize(content);
            await File.WriteAllTextAsync(path, serializedContent, cancellationToken);
        }
    }
}
