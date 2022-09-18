using OpenFilePluginBase;
using System.Diagnostics;

namespace WordPadOpenPlugin
{
    [Extension(".txt")]
    internal class WordPadOpener : IOpenFile
    {
        public Task EditAsync(string path)
        {
            return this.OpenAsync(path);
        }

        public Task OpenAsync(string path)
        {
            return Task.Run(() =>
            {
                this.startProcess(path);
            });
        }

        private void startProcess(string path)
        {
            var pi = new ProcessStartInfo
            {
                FileName = @"C:\Program Files\Windows NT\Accessories\wordpad.exe",
                Arguments = path,
                CreateNoWindow = true,
                UseShellExecute = false,
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
