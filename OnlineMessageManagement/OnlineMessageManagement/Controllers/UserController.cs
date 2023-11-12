using Microsoft.AspNetCore.Mvc;
using OnlineMessageManagement.DataAccess;
using OnlineMessageManagement.Models;
using Microsoft.AspNetCore.Authorization;


namespace OnlineMessageManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository;
     

        public UserController(IConfiguration configuration)
        {
            _userRepository = new UserRepository(configuration);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(decimal loginId, string password)
        {
            HttpContext.Session.SetInt32("_loginId", (int)loginId); // Store loginId in session

            Logpannel user = _userRepository.LoginIdAndPassword(loginId, password);

            if (user != null)
            {
                return RedirectToAction("SocialServiceList", "SocialService");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid login credentials.";
                return View("Login");
            }
        }

        [HttpPost]
        public IActionResult LogUserDetails()
        {
            int? storedLoginId = HttpContext.Session.GetInt32("_loginId");
            if (storedLoginId.HasValue)
            {
                Logpannel userinfo = _userRepository.LogInfo((decimal)storedLoginId);

                if (userinfo != null)
                {
                    return Json(userinfo);
                }
            }

            return Json(new { errorMessage = "Invalid login User." });
        }


        public IActionResult Logout()
        {
          
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
