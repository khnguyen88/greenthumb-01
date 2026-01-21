using AgenticGreenthumbApi.Domain;
using DocumentFormat.OpenXml.Office2013.Word;
using NRedisStack.Search;
using System.Linq;
using System.Text.Json;

namespace AgenticGreenthumbApi.Helper
{
    public static class FileReaderHelper
    {
        public static string GetFileFromDirectory(string[] subDirectoryPath, string filename)
        {
            string subPath = Path.Combine(subDirectoryPath);
            string path = Path.Combine(Environment.CurrentDirectory, subPath, filename);
            return GetFile(path, filename);
        }

        public static string GetFileFromDirectory(string subDirectoryPath, string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, subDirectoryPath, filename);
            return GetFile(path, filename);
        }

        public static string GetContextFile(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Semantic", "Contexts", filename);
            return GetFile(path, filename);
        }

        public static string GetDomainFile(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Domain", filename);
            return GetFile(path, filename);
        }

        public static string GetModelFile(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Models", filename);
            return GetFile(path, filename);
        }

        private static string GetFile(string path, string filename)
        {
            try
            {
                if (File.Exists(path))
                {
                    var reader = File.OpenText(path);
                    return reader.ReadToEnd();
                }

                Console.WriteLine($"Context document '{filename}' missing.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            return "";
        }

        public static List<AgentTemplate> GetAgentTemplates(params string[] subDirectories)
        {
            string[] directories = { Environment.CurrentDirectory };
            directories = directories.Concat(subDirectories).ToArray();

            string path = Path.Combine(directories);

            List<AgentTemplate> agentTemplates = new List<AgentTemplate>();
            try
            {
                string[] files = Directory.GetFiles(path, "*.json");

                if (files.Length > 0)
                {
                    foreach (string file in files)
                    {
                        try
                        {
                            string fileContent = File.ReadAllText(file);

                            AgentTemplate agentTemplate = JsonSerializer.Deserialize<AgentTemplate>(fileContent);
                            if (agentTemplate != null)
                            {
                                agentTemplates.Add(agentTemplate);

                                Console.WriteLine($"Successfully read the agent template file, {file}, for the {agentTemplate.Name}");
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

            Console.WriteLine($"There are no agent template files found in the following path: '{path}' .");
            return agentTemplates;
        }
    }
}
