using Microsoft.AspNetCore.Mvc;
using TestApi.Model;
using TestApi.Service.Interface;

namespace TestApi.Controllers
{
    [ApiController]
    [Route("report")]
    public class ReportController (IReportService reportService): Controller
    {
        [HttpPost, Route("user_stastic")]
        public async Task<ActionResult> GetUserStatistic([FromBody] UserStatisticsRequest request)
        {
            var guid = await reportService.StartUserStatisticsAsync(request.UserId, request.StartDateTime, request.EndDateTime);
            return Ok(guid);
        }
        [HttpGet("info")]
        public async Task<IActionResult> GetRequestInfo([FromQuery] Guid guid)
        {
            var requestInfo = await reportService.GetRequestInfoAsync(guid);
            if (requestInfo == null)
            {
                return NotFound();
            }

            var percentage = reportService.GetProcessingPercentage(requestInfo);

            return Ok(new
            {
                Gujd = requestInfo.Guid,
                Status = requestInfo.Status,
                Percentage = percentage,
                Result = requestInfo.Status == "Завершен" ? requestInfo.Result : null
            });
        }
    }
}
