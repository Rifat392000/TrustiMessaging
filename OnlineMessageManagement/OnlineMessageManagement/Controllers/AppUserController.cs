
using Microsoft.AspNetCore.Mvc;
using OnlineMessageManagement.Data;
using OnlineMessageManagement.Models;
using OnlineMessageManagement.ViewModels;

namespace OnlineMessageManagement.Controllers
{
    
    public class AppUserController : Controller
    {
        private readonly AppUserDataAccess _appUserDataAccess;

        public AppUserController(AppUserDataAccess appUserDataAccess)
        {
            _appUserDataAccess = appUserDataAccess;
        }

        public IActionResult msgcreate()
        {
            var socialServices = _appUserDataAccess.GetAllSocialServices();

            var viewModel = new AppUserViewModel
            {
                SocialServices = socialServices
            };

            return View(viewModel);
        }

        
        public IActionResult GetCustomersByService(int serviceId)
        {
            var customers = _appUserDataAccess.GetAppUsers(serviceId);
            return Json(customers);
        }


        [HttpPost]
        public IActionResult InsertSingleMessage([FromBody] SingleMessage message)
        {
            if (ModelState.IsValid)
            {
                
                _appUserDataAccess.InsertSingleMessage(message.Cid, message.Cname, message.PhoneNumber, message.ServiceName, message.ServiceId, message.CustomerMessage);

                return Json(new { success = true, message = "Message sent successfully" });
            }
            else
            {
                return Json(new { success = false, message = "Invalid message data" });
            }
        }

        public IActionResult GetSingleMessages()
        {
            var singleMessages = _appUserDataAccess.GetSingleMessages();
            return View(singleMessages);
        }


        public IActionResult GetbulkMessages()
        {
            var bulkMessages = _appUserDataAccess.GetbulkMessages();
            return View(bulkMessages);
        }


        public IActionResult bulkmsgcreate()
        {
            var socialServices = _appUserDataAccess.GetAllSocialServices();

            var viewModel = new AppUserViewModel
            {
                SocialServices = socialServices
            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult InsertbulkMessagecontroller([FromBody] MessageModel message)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(message.ServiceIds))
                {
                    int[] serviceIdArray = message.ServiceIds.Split(',').Select(int.Parse).ToArray();

                    foreach (int serviceId in serviceIdArray)
                    {
                        Console.WriteLine(serviceId);
                        Console.WriteLine(message.CustomerMessage);
                        _appUserDataAccess.InsertbulkMessage(serviceId, message.CustomerMessage);
                    }

                    return Json(new { success = true, message = "Bulk message sent successfully" });
                }
                else
                {
                    return Json(new { success = false, message = "No services selected" });
                }
            }
            else
            {
                return Json(new { success = false, message = "Invalid bulk message data" });
            }
        }




        public IActionResult SingleMessageUncheckeds()
        {
            var singleMessages = _appUserDataAccess.SingleMessageUncheckeds();
            return View(singleMessages);
        }

        public IActionResult bulkMessageUncheckeds()
        {
            var bulkMessages = _appUserDataAccess.bulkMessageUncheckeds();
            return View(bulkMessages);
        }







        [HttpPost]
        public IActionResult ProcessSingleMessages([FromBody] List<MessageModel> data)
        {
            try
            {
                foreach (var item in data)
                {
                    // Process each item in the list
                    _appUserDataAccess.SingleMessagecheckedStatus(item.logSingleMessage, item.checkedmsg, item.messageStatus);
                }

                return Json(new { success = true, message = "Bulk messages processed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error processing bulk messages: " + ex.Message });
            }
        }




        [HttpPost]
        public IActionResult ProcessBulkMessages([FromBody] List<MessageModel> data)
        {
            try
            {
                foreach (var item in data)
                {
                    // Process each item in the list
                    _appUserDataAccess.bulkMessagecheckedStatus(item.logSingleMessage, item.checkedmsg, item.messageStatus);
                }

                return Json(new { success = true, message = "Bulk messages processed successfully." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error processing bulk messages: " + ex.Message });
            }
        }

        public IActionResult BulkMessageInfoCombined()
        {
            var bulkMessageInfoByService = _appUserDataAccess.BulkMessageInfoByService();
            var sendTimeForBulkMessages = _appUserDataAccess.SendTimeForBulkMessages();
            var singleMessageInfoByService = _appUserDataAccess.SingleMessageInfoByService();
            var sendTimeForSingleMessages = _appUserDataAccess.SendTimeForSingleMessages();

            var combinedData = new Tuple<List<MessageInfoByService>, List<MessageInfoByService>, List<MessageInfoByService>, List<MessageInfoByService>>(
                bulkMessageInfoByService, sendTimeForBulkMessages, singleMessageInfoByService, sendTimeForSingleMessages);

            return View(combinedData);
        }







    }
}
