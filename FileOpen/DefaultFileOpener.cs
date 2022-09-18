using OpenFilePluginBase;
using System.Diagnostics;

namespace FileOpen
{
    internal class DefaultFileOpener : IOpenFile
    {
        public Task EditAsync(string path)
        {
            return Task.Run(() =>
            {
                this.startProcess(path, "edit");
            });
        }

        public Task OpenAsync(string path)
        {
            return Task.Run(() =>
            {
                this.startProcess(path, "open");
            });
        }

        private void startProcess(string path, string verb)
        {
            var pi = new ProcessStartInfo
            {
                FileName = path,
                Verb = verb,
                UseShellExecute = true,
            };
            using var p = new Process
            {
                StartInfo = pi,
            };
            p.Start();
            p.WaitForExit();
        }
    }
}
