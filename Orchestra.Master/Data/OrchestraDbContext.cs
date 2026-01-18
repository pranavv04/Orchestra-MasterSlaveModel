using Microsoft.EntityFrameworkCore;
using Orchestra.Master.Models;

namespace Orchestra.Master.Data;

public class OrchestraDbContext : DbContext
{
    public OrchestraDbContext(DbContextOptions<OrchestraDbContext> options)
        : base(options)
    {
    }

    public DbSet<Job> Jobs => Set<Job>();
}
