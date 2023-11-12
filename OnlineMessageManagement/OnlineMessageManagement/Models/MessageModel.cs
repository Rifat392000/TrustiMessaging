namespace OnlineMessageManagement.Models
{
    public class MessageModel
    {
        public string? ServiceIds { get; set; }
        public string? CustomerMessage { get; set; }

        public int logSingleMessage { get; set; }
        public int checkedmsg { get; set; }
        public int messageStatus { get; set; }
    }


}
