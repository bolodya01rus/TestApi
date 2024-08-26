namespace TestApi.Model
{
    public class UserStatisticsRequest
    {
        public string? UserId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
    }
}
