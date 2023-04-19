using DocsWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<SynchronizerWorker>();
    })
    .Build();

host.Run();
