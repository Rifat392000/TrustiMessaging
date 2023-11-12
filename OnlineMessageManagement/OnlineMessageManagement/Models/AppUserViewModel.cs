using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.ViewModels
{
    public class AppUserViewModel
    {
        public List<AppUserModel>? AppUsers { get; set; }
        public List<SocialService>? SocialServices { get; set; }

        public List<SingleMessage>? SingleMessages { get; set; }
        public List<bulkMessage>? bulkMessages { get; set; }
    }
}
