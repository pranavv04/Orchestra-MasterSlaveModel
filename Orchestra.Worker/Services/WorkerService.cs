using System.Diagnostics;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using Orchestra.Worker.Models;

namespace Orchestra.Worker.Services
{
    public class WorkerService : BackgroundService
    {
        private readonly HttpClient _http;
        private readonly string _workerId;
        private readonly string _workingDir =
            "/home/pranav/Projects/Dotnet/Orchestra/WorkerFiles";

        public WorkerService(HttpClient http)
        {
            _http = http;
            _workerId = Guid.NewGuid().ToString();
            Console.WriteLine($"Worker started: {_workerId}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Job? job = null;

                try
                {
                    var response = await _http.PostAsJsonAsync(
                        "http://localhost:5216/api/job/claim",
                        _workerId,
                        stoppingToken);

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        await Task.Delay(2000, stoppingToken);
                        continue;
                    }

                    job = await response.Content.ReadFromJsonAsync<Job>(cancellationToken: stoppingToken);
                    if (job == null) continue;

                    var process = new Process
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "/bin/bash",
                            Arguments = $"-c \"cd {_workingDir} && {job.Command}\"",
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false
                        }
                    };

                    process.Start();

                    string stdout = await process.StandardOutput.ReadToEndAsync();
                    string stderr = await process.StandardError.ReadToEndAsync();

                    await process.WaitForExitAsync(stoppingToken);

                    job.Output = stdout + stderr;
                    job.Status = process.ExitCode == 0
                        ? JobStatus.Completed
                        : JobStatus.Failed;
                }
                catch (Exception ex)
                {
                    if (job != null)
                    {
                        job.Status = JobStatus.Failed;
                        job.Output = ex.ToString();
                    }
                }

                if (job != null)
                {
                    await _http.PostAsJsonAsync(
                        "http://localhost:5216/api/job/update",
                        job,
                        stoppingToken);
                }
            }
        }
    }
}
