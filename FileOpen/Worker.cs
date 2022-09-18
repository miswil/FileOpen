namespace FileOpen
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private ConfigurationFileOpener _configurationFileOpener;
        private DefaultFileOpener _defaultFileOpener;
        private DelegatePluginFileOpener _delegatePluginFileOpener;

        public Worker(ILogger<Worker> logger, FileOpenConfigurations fileOpenConfigurations)
        {
            _logger = logger;
            this._configurationFileOpener = new(fileOpenConfigurations);
            this._defaultFileOpener = new();
            this._delegatePluginFileOpener = new("plugins");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            this._delegatePluginFileOpener.LoadPlugins();
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine("Input a file path.");
                var path = Console.ReadLine();
                await this.OpenFileAsync(path);
            }
        }

        private Task OpenFileAsync(string path)
        {
            var ext = Path.GetExtension(path);
            if (this._delegatePluginFileOpener.Support(ext))
            {
                return this._delegatePluginFileOpener.OpenAsync(path);
            }
            else if (this._configurationFileOpener.Support(ext))
            {
                return this._configurationFileOpener.OpenAsync(path);
            }
            else
            {
                return this._defaultFileOpener.OpenAsync(path);
            }
        }
    }
}