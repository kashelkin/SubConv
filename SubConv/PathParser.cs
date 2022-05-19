namespace SubConv
{
    public static class PathParser
    {
        public static IEnumerable<string> GetFiles(string path)
        {
            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrWhiteSpace(directory))
                directory = ".";

            var fileMask = Path.GetFileName(path);

            return Directory.GetFiles(directory, fileMask)
                .Select(RemoveDotSlash);
        }

        public static string GenerateOutputFileName(string inputFile, string? outputPath, string extension)
        {
            var outputFile = Path.ChangeExtension(Path.GetFileNameWithoutExtension(inputFile), extension);

            var outputPathFixed = string.IsNullOrWhiteSpace(outputPath)
                ? Path.GetDirectoryName(inputFile) ?? ""
                : outputPath;

            return Path.Combine(outputPathFixed, outputFile);
        }

        private static string RemoveDotSlash(string path) =>
            path.StartsWith($".{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                ? path[2..]
                : path;
    }
}
