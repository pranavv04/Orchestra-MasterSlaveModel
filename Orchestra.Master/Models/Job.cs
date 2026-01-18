namespace Orchestra.Master.Models;


public class Job
{
    public int Id{get; set;}
    public string Command {get; set;} = string.Empty;
    public JobStatus Status{get;set;} = JobStatus.Pending;
    public string? AssignedWorker{get;set;}
    public string? Output{get; set;}
    public int RetryCount {get;set;} = 0;
    public int maxRetries {get;set;} = 3;
    public DateTime CreatedAt{get; set;} = DateTime.UtcNow;
    
}