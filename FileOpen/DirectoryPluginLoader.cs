using System.Reflection;

namespace FileOpen
{
    internal sealed class DirectoryPluginLoader : IDisposable
    {
        private List<PluginLoadContext> _contexts = new();
        private string _pluginsRelativePath;

        public DirectoryPluginLoader(string pluginsRelativePath)
        {
            this._pluginsRelativePath = pluginsRelativePath;
        }

        public void Dispose()
        {
            this._contexts.ForEach(c => c.Unload());
        }

        public IEnumerable<Assembly> Load()
        {
            var pluginAbsolutePath = Path.Combine(
                Path.GetDirectoryName(
                    Assembly.GetExecutingAssembly().Location),
                this._pluginsRelativePath);
            var pluginDir = new DirectoryInfo(pluginAbsolutePath);
            foreach (var dir in pluginDir.GetDirectories())
            {
                var pluginName = dir.Name;
                var pluginPath = Path.Combine(pluginAbsolutePath, pluginName, pluginName + ".dll");
                var context = new PluginLoadContext(pluginPath);
                this._contexts.Add(context);
                yield return context.LoadFromAssemblyName(new(pluginName));
            }
        }
    }
}
