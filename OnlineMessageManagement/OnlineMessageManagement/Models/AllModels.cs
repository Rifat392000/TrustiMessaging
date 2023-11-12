using System.Collections.Generic;

namespace OnlineMessageManagement.Models
{
    public class AllModels
    {
        public List<SocialService> socialServiceList { get; set; }
        public int TotalServiceUserCount { get; set; }
        public Dictionary<int, int> ServiceUserCounts { get; set; }
    }
}
