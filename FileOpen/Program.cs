using FileOpen;

IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<FileOpenConfigurations>();
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
