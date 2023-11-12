using Microsoft.AspNetCore.Mvc;
using OnlineMessageManagement.Models;
using OnlineMessageManagement.Services;
using System.Net;

namespace OnlineMessageManagement.Controllers
{
    public class SocialServiceController : Controller
    {
        private readonly ISocialServiceServices _socialServiceServices;

        public SocialServiceController(ISocialServiceServices socialServiceServices)
        {
            _socialServiceServices = socialServiceServices;
        }



        [HttpGet]
        public IActionResult SocialServiceList()
        {
            AllModels model = new AllModels();

            model.socialServiceList = _socialServiceServices.GetSocialServiceRecord();
            model.TotalServiceUserCount = _socialServiceServices.GetTotalServiceUserCount();
            model.ServiceUserCounts = _socialServiceServices.GetServiceUserCountByService();

            

            return View(model);
        }
        public IActionResult Settings()
        {
            List<SocialService> socialServices = _socialServiceServices.GetSocialServiceRecord();
            string hostName = Dns.GetHostName();
            ViewBag.HostName = hostName;

            IPAddress[] addresses = Dns.GetHostAddresses(hostName);
            foreach (IPAddress address in addresses)
            {
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    string myIPv4 = address.ToString();
                    ViewBag.IPv4Address = myIPv4;
                    break;
                }
            }
            return View(socialServices);
        }

        [HttpPost]
        public IActionResult UpdateServiceStatus(int serviceId, int status)
        {
            _socialServiceServices.UpdateServiceStatus(serviceId, status);
            return Json(new { status });
        }


        [HttpGet]
        public IActionResult AddSocialService()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddSocialService(string serviceName)
        {
            try
            {
                _socialServiceServices.AddSocialService(serviceName);
                return RedirectToAction(nameof(Settings));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        [HttpGet]
        public IActionResult UpdateService(int  id)
        {
            // Fetch the service details based on the id
            Console.WriteLine(id);
            SocialService service = _socialServiceServices.GetServiceById(id);

            if (service == null)
            {
                // Handle case where service is not found
                return NotFound();
            }

            return View(service);
        }


        [HttpPost]
        public IActionResult UpdateService(int id, string serviceName)
        {
            try
            {
                _socialServiceServices.UpdateService(id, serviceName);
                return RedirectToAction(nameof(Settings));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

   

        [HttpGet]
        public IActionResult DeleteService(int id)
        {
            
            SocialService service = _socialServiceServices.GetServiceById(id);

            if (service == null)
            {
                
                return NotFound();
            }

            return View(service);
        }

        [HttpPost]
        public IActionResult DeleteServiceConfirmed(int id)
        {
            try
            {
                _socialServiceServices.DeleteService(id);
                return RedirectToAction(nameof(Settings));
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        


    }
}
