namespace Api.Extensions
{
    public static class ConfigurationExtension
    {
        public static ConfigurationManager AddConfigurationExtension
            (this ConfigurationManager configuration,
            string environment)
        {
            configuration.SetBasePath(DirectoryExtension.GetDirectoryPath());
            var appSettingsPath = environment.ToLower() switch
            {
                "development" => "appsettings.development.json",
                "production" => "appsettings.production.json",
                _ => "appsettings.json",
            };

            configuration.AddAppSettings(appSettingsPath);

            return configuration;
        }

        private static IConfigurationBuilder AddAppSettings(this IConfigurationBuilder configuration, string path)
            => configuration.AddJsonFile
            (
                path,
                optional: true,
                reloadOnChange: true
                );
    }
}
