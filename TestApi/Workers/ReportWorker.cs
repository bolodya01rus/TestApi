
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TestApi.Classes;
using TestApi.Model;

namespace TestApi.Workers
{
    public class ReportWorker(IDbContextFactory<DataContext> contextFactory, IConfiguration configuration) : BackgroundService
    {
        private readonly int _timeOutMilliseconds = configuration.GetValue("TimeOutMilliseconds", 60000);
        private readonly List<CancellationTokenSource> _cancellationTokenSources = new();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                int queue = Queues.RequestInfoQueue.Count;
                if (queue > 0)
                {
                     AddTask(queue);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }

        private void AddTask(int queue)
        {
            for (int i = 0; i < queue; i++)
            {
                CancellationTokenSource cancellationTokenSource = new();
                _ = RunTask(cancellationTokenSource.Token);
                _cancellationTokenSources.Add(cancellationTokenSource);
            }
        }

        private async Task RunTask(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Queues.RequestInfoQueue.TryDequeue(out var requestInfo);
                if (requestInfo != null)
                {
                    await using var context = await contextFactory.CreateDbContextAsync(stoppingToken);
                    context.RequestInfos.Add(requestInfo);
                    await context.SaveChangesAsync(stoppingToken);

                    await Task.Delay(_timeOutMilliseconds, stoppingToken);

                    requestInfo.EndTime = DateTime.UtcNow;
                    requestInfo.Status = "Завершен";
                    var count = context.RequestInfos.Count(e => e != null && e.UserId == requestInfo.UserId);

                    requestInfo.Result = JsonSerializer.Serialize(new RequestResult(){Count = count,UserId = requestInfo.UserId});

                    context.RequestInfos.Update(requestInfo);
                    await context.SaveChangesAsync(stoppingToken);
                }

                await Task.Delay(1, CancellationToken.None);
            }
        }
    }
}
