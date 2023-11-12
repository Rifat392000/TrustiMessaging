namespace OnlineMessageManagement.Models
{
    public class MessageInfoByService
    {
        public string? ServiceName { get; set; }
        public int Total { get; set; }
        public int Process { get; set; }
        public int Pending { get; set; }
        public int Success { get; set; }
        public int Failed { get; set; }

        public string? DistinctDate { get; set; }
        public int NumMessagesPerMinute { get; set; }
    }

}
