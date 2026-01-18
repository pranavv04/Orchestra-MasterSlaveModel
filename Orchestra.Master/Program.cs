using Microsoft.EntityFrameworkCore;
using Orchestra.Master.Data;
using Orchestra.Master.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString =
    "server=localhost;database=Orchestra;user=root;password=pranav";

builder.Services.AddDbContext<OrchestraDbContext>(options =>
{
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );
});

var workerId = Guid.NewGuid().ToString();
builder.Services.AddSingleton(new WorkerInfo(workerId));
builder.Services.AddGrpc();
builder.Services.AddHostedService<Orchestra.Master.Services.JobScheduler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
