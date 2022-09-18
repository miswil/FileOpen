using OpenFilePluginBase;
using System.Diagnostics;

namespace FileOpen
{
    internal class ConfigurationFileOpener : IOpenFile
    {
        private readonly FileOpenConfigurations _configurations;

        public ConfigurationFileOpener(FileOpenConfigurations configurations)
        {
            this._configurations = configurations;
        }

        public IEnumerable<string> SupportedExtensions
            => this._configurations.Configurations.Select(c => c.Extension);

        public bool Support(string extension)
        {
            return this._configurations.Configurations.FirstOrDefault(c => c.Extension == extension) is not null;
        }

        public Task EditAsync(string path)
        {
            var extension = Path.GetExtension(path);
            var command = this._configurations.Configurations.FirstOrDefault(c => c.Extension == extension);
            if (command is null)
            {
                throw new NotImplementedException();
            }
            var editCommand = string.Format(command.Edit, path);
            return Task.Run(() =>
            {
                this.startProcess(editCommand);
            });
        }

        public Task OpenAsync(string path)
        {
            var extension = Path.GetExtension(path);
            var command = this._configurations.Configurations.FirstOrDefault(c => c.Extension == extension);
            if (command is null)
            {
                throw new NotImplementedException();
            }
            var openCommand = string.Format(command.Open, path);
            return Task.Run(() =>
            {
                this.startProcess(openCommand);
            });
        }

        private void startProcess(string command)
        {
            var pi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/c {command}",
            };
            using var p = new Process
            {
                StartInfo = pi,
            };
            p.Start();
            p.WaitForExit();
        }
        private class FileOpenCommand
        {
            public FileOpenCommand(string open, string edit)
            {
                this.Open = open;
                this.Edit = edit;
            }

            public string Open { get; set; }
            public string Edit { get; set; }
        }
    }
}
