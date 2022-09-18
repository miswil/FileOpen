using OpenFilePluginBase;

namespace FileOpen
{
    internal class DelegatePluginFileOpener : IOpenFile
    {
        private readonly FileOpenPluginLoader _loader;
        private readonly Dictionary<string, Type> _plugins = new();


        public DelegatePluginFileOpener(string pluginsRelativePath)
        {
            this._loader = new(pluginsRelativePath);
        }

        public IEnumerable<string> SupportedExtensions
            => this._plugins.Keys;

        public bool Support(string extension)
        {
            return this._plugins.ContainsKey(extension);
        }

        public void LoadPlugins()
        {
            this._plugins.Clear();
            foreach (var plugin in this._loader.Load())
            {
                var types = plugin.GetTypes().Where(t => typeof(IOpenFile).IsAssignableFrom(t));
                foreach (var type in types)
                {
                    var supportedExtensions = type.GetCustomAttributes(true).OfType<ExtensionAttribute>();
                    foreach (var supportedExtension in supportedExtensions)
                    {
                        try
                        {
                            this._plugins.Add(supportedExtension.Extension, type);
                        }
                        catch (ArgumentException)
                        { }
                    }
                }
            }
        }

        public Task EditAsync(string path)
        {
            var extension = Path.GetExtension(path);
            var plugin = this.InstantiatePlugin(extension);
            return plugin.EditAsync(path);
        }

        public Task OpenAsync(string path)
        {
            var extension = Path.GetExtension(path);
            var plugin = this.InstantiatePlugin(extension);
            return plugin.OpenAsync(path);
        }

        private IOpenFile InstantiatePlugin(string extension)
        {
            return (IOpenFile)Activator.CreateInstance(this._plugins[extension]);
        }
    }
}
