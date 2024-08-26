using TestApi.Model;

namespace TestApi.Service.Interface
{
    public interface IReportService
    {
        Task<Guid> StartUserStatisticsAsync(string? userId, DateTime start, DateTime end);
          Task<RequestInfo?> GetRequestInfoAsync(Guid guid);
          double GetProcessingPercentage(RequestInfo requestInfo);



    }
}
