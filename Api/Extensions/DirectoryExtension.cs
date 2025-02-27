namespace Api.Extensions
{
    public static class DirectoryExtension
    {
        public static string GetDirectoryPath(string? directory = null)
            => !string.IsNullOrWhiteSpace(directory)
            ? directory
            : Path.GetDirectoryName(Environment.ProcessPath)
            ?? string.Empty;
    }
}
