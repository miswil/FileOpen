using System.Text.Json;

namespace FileOpen
{
    internal class FileOpenConfigurations
    {
        public FileOpenConfigurations(IConfiguration configuration)
        {
            var json = configuration.GetSection(nameof(FileOpenConfigurations)).Value;
            if (json is null)
            {
                return;
            }
            this.Configurations.AddRange(JsonSerializer.Deserialize<IEnumerable<FileOpenConfiguration>>(json));
        }
        public List<FileOpenConfiguration> Configurations { get; set; } = new();
    }

    internal class FileOpenConfiguration
    {
        public string Extension { get; set; }
        public string Open { get; set; }
        public string Edit { get; set; }
    }
}
