using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orchestra.Master.Data;
using Orchestra.Master.Models;

namespace Orchestra.Master.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobController: ControllerBase
{
    private readonly OrchestraDbContext _db;
    

    public JobController(OrchestraDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<IActionResult> CreateJob(Job job)
    {
        job.Status = JobStatus.Pending;
        job.CreatedAt = DateTime.UtcNow;

        _db.Jobs.Add(job);
        await _db.SaveChangesAsync();
        return Ok(job);
    }

     [HttpGet("pending")]
    public async Task<IActionResult> GetPendingJobs()
    {
        var jobs = await _db.Jobs
            .Where(j => j.Status == JobStatus.Pending)
            .OrderBy(j => j.CreatedAt)
            .ToListAsync();
        return Ok(jobs);
    }

[HttpPost("claim")]
public async Task<IActionResult> ClaimJob([FromBody] string workerId)
{
    using var transaction = await _db.Database.BeginTransactionAsync();

    var job = await _db.Jobs
        .Where(j => j.Status == JobStatus.Pending)
        .OrderBy(j => j.CreatedAt)
        .FirstOrDefaultAsync();

    if (job == null)
        return NoContent();

    job.Status = JobStatus.Running;
    job.AssignedWorker = workerId;

    await _db.SaveChangesAsync();
    await transaction.CommitAsync();

    return Ok(job);
}


    
    [HttpPost("update")]
    public async Task<IActionResult> UpdateJob([FromBody] Job job)
    {
        var existingJob = await _db.Jobs.FindAsync(job.Id);
        if (existingJob == null) return NotFound();

        existingJob.Status = job.Status;
        existingJob.Output = job.Output;
        existingJob.AssignedWorker = job.AssignedWorker;
        existingJob.RetryCount = job.RetryCount;

        await _db.SaveChangesAsync();
        return Ok(existingJob);
    }

    [HttpGet]
    public async Task<IActionResult> GetJobs()
    {
        var listJobs =  await _db.Jobs.ToListAsync();
        return Ok(listJobs);

    }
[HttpGet("{id:int}")]
public async Task<IActionResult> GetJob(int id)
{
    var job = await _db.Jobs.FindAsync(id);
    if (job is null)
        return NotFound("Job not found");

    return Ok(job);
}


[HttpDelete("{id:int}")]
public async Task<IActionResult> DeleteJob(int id)
{
    var job = await _db.Jobs.FindAsync(id);
    if (job is null)
        return NotFound("Job not found");

    _db.Jobs.Remove(job);
    await _db.SaveChangesAsync();
    return Ok("Job Deleted");
}

}