using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestra.Worker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Register HttpClient for WorkerService
        services.AddHttpClient<WorkerService>();

        // Register WorkerService as a hosted service
        services.AddHostedService<WorkerService>();
    })
    .Build();

await host.RunAsync();
