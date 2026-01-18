using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orchestra.Worker.Services;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        
        services.AddHttpClient<WorkerService>();

        
        services.AddHostedService<WorkerService>();
    })
    .Build();

await host.RunAsync();
