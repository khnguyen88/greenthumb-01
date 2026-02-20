using AgenticGreenthumbApi.Domain;
using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.IdentityModel.Tokens;
using Microsoft.KernelMemory.DataFormats;
using NRedisStack.Search;
using System.Linq;
using System.Text.Json;

namespace AgenticGreenthumbApi.Helper
{
    public static class FileHelper
    {
        public static string BuildPathFromProjectDirectory(string[] subDirectoryPath)
        {
            string subPath = Path.Combine(subDirectoryPath);
            return Path.Combine(Environment.CurrentDirectory, subPath);
        }

        public static string BuildFilePath(string directoryPath, string fileName)
        {
            return Path.Combine(directoryPath, fileName);
        }

        public static Task<string> GetFileFromDirectoryAsync(string[] subDirectoryPath, string filename)
        {
            string subPath = Path.Combine(subDirectoryPath);
            string path = Path.Combine(Environment.CurrentDirectory, subPath, filename);
            return GetFileAsync<string>(path);
        }

        public static Task<string> GetFileFromDirectoryAsync(string subDirectoryPath, string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, subDirectoryPath, filename);
            return GetFileAsync<string>(path);
        }

        public static Task<string> GetFileFromPathAsync(string fullPath, string filename)
        {
            string filePath = Path.Combine(fullPath, filename);
            return GetFileAsync<string>(filePath);
        }

        public static Task<string> GetFileFromPathAsync(string fullFilePath)
        {
            return GetFileAsync<string>(fullFilePath);
        }

        public static Task<string> GetContextFileAsync(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Semantic", "Contexts", filename);
            return GetFileAsync<string>(path);
        }

        public static Task<string> GetDomainFileAsync(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Domain", filename);
            return GetFileAsync<string>(path);
        }

        public static Task<string> GetModelFileAsync(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Models", filename);
            return GetFileAsync<string>(path);
        }

        private static async Task<T?> GetFileAsync<T>(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    string fileString = await File.ReadAllTextAsync(path);

                    if (typeof(T) == typeof(string))
                    {
                        return (T)(object)fileString;
                    }

                    return JsonSerializer.Deserialize<T>(fileString);
                }
                else
                {
                    Console.WriteLine($"Context document '{Path.GetFileName(path)}' missing.");
                    return default;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");

                return default;
            }
        }

        public async static Task<List<T>> GetTemplateFilesAsync<T>(params string[] subDirectories)
        {
            List<T> templates = await GetFilesAsync<T>(subDirectories);

            if (templates.IsNullOrEmpty())
            {
                return new List<T>();
            }

            return templates;
        }

        public async static Task<List<T>> GetFilesAsync<T>(params string[] subDirectories)
        {
            string[] directories = { Environment.CurrentDirectory };
            directories = directories.Concat(subDirectories).ToArray();

            string path = Path.Combine(directories);

            List<T> templates = new List<T>();
            try
            {
                string[] files = Directory.GetFiles(path, "*.json");

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        try
                        {
                            string fileContent = await File.ReadAllTextAsync(file);

                            T agentTemplate = JsonSerializer.Deserialize<T>(fileContent);
                            if (agentTemplate is not null)
                            {
                                templates.Add(agentTemplate);

                                Console.WriteLine($"Successfully read the template file, {file}.");
                            }
                        }
                        catch (JsonException ex)
                        {
                            Console.WriteLine($"Error deserializing file {file}: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }

            Console.WriteLine($"There are no template files found in the following path: '{path}' .");
            return templates;
        }
    }
}
