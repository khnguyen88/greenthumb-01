namespace AgenticGreenthumbApi.Helper
{
    public static class FileReaderHelper
    {
        public static string GetContextFile(string filename)
        {
            string path = Path.Combine(Environment.CurrentDirectory, "Semantic", "Contexts", filename);
            if (File.Exists(path))
            {
                var reader = File.OpenText(path);
                return reader.ReadToEnd();
            }

            Console.WriteLine($"Context document '{filename}' missing.");
            return "";
        }
    }
}
