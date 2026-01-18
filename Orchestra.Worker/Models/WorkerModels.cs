namespace Orchestra.Worker.Models
{
    public enum JobStatus
    {
        Pending=0,
        Assigned = 1,
        Running=2,
        Completed=3,
        Failed=4
    }

    public class WorkerInfo
    {
        public string WorkerId { get; }
        public WorkerInfo(string workerid)
        {
            WorkerId = workerid;
        }
    }
}
