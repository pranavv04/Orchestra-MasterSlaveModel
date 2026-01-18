
using Microsoft.EntityFrameworkCore;
using Orchestra.Master.Data;
using Orchestra.Master.Models;

namespace Orchestra.Master.Services;

public class JobScheduler : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly WorkerInfo _workerInfo;



    public JobScheduler(IServiceScopeFactory scopeFactory, WorkerInfo workerInfo)
    {
        _scopeFactory = scopeFactory;
        _workerInfo = workerInfo;
    }


   protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    while (!stoppingToken.IsCancellationRequested)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<OrchestraDbContext>();

        var job = await db.Jobs
          .Where(j => j.Status == JobStatus.Pending)
          .OrderBy(j => j.CreatedAt)
          .FirstOrDefaultAsync(stoppingToken);

        if (job != null)
        {
            job.Status = JobStatus.Assigned;
            job.AssignedWorker = _workerInfo.WorkerId;
            await db.SaveChangesAsync(stoppingToken);
        }

        await Task.Delay(2000, stoppingToken); 
    }
}

}
