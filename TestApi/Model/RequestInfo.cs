namespace TestApi.Model
{
    public class RequestInfo
    {
        public Guid Guid { get; set; }
        public string? UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
        public string? Result { get; set; }
    }
}
