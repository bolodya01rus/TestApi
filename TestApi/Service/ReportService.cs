using Microsoft.EntityFrameworkCore;
using TestApi.Classes;
using TestApi.Model;
using TestApi.Service.Interface;

namespace TestApi.Service
{
    public class ReportService(IDbContextFactory<DataContext> contextFactory, IConfiguration configuration) : IReportService
    {
        private readonly int _timeOutMilliseconds = configuration.GetValue("TimeOutMilliseconds", 60000);

        public async Task<Guid> StartUserStatisticsAsync(string? userId, DateTime startDate, DateTime endDate)
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            var requestInfo = new RequestInfo
            {
                Guid = Guid.NewGuid(),
                UserId = userId,
                StartTime = DateTime.UtcNow,
                Status = "В процессе"
            };
Queues.RequestInfoQueue.Enqueue(requestInfo);
            return requestInfo.Guid;
        }

        public async Task<RequestInfo?> GetRequestInfoAsync(Guid guid)
        {
            await using var context = contextFactory.CreateDbContext();

            return await context.RequestInfos.FindAsync(guid);
        }

        public double GetProcessingPercentage(RequestInfo requestInfo)
        {
            if (requestInfo.Status == "Завершен")
            {
                return 100.0;
            }

            var elapsedMilliseconds = (DateTime.UtcNow - requestInfo.StartTime).TotalMilliseconds;
            return Math.Min(100.0, Math.Round(elapsedMilliseconds / _timeOutMilliseconds * 100));
        }
    }
}
