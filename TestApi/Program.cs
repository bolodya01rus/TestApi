using Microsoft.EntityFrameworkCore;
using TestApi.Service;
using TestApi.Service.Interface;
using TestApi.Workers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Configuration.AddJsonFile($"{AppContext.BaseDirectory}\\appsettings.json");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddHostedService<ReportWorker>();
builder.Services.AddDbContextFactory<DataContext>(options=>options.UseSqlite($"Data Source=report.db"));
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
