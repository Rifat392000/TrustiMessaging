using Microsoft.AspNetCore.Mvc;

namespace OnlineMessageManagement.Controllers
{
    public class MessageBoxController: Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
