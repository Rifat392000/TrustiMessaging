using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OnlineMessageManagement.Data;
using OnlineMessageManagement.Models;

namespace OnlineMessageManagement.Controllers
{
    public class ServiceUserController : Controller
    {
        private readonly ServiceUserRepository _serviceUserRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly SocialServiceRepository _socialServiceRepository;

        public ServiceUserController(
            ServiceUserRepository serviceUserRepository,
            CustomerRepository customerRepository,
            SocialServiceRepository socialServiceRepository)
        {
            _serviceUserRepository = serviceUserRepository;
            _customerRepository = customerRepository;
            _socialServiceRepository = socialServiceRepository;
        }

        public IActionResult Index()
        {
            var socialServices = _socialServiceRepository.GetAll(); ; // Retrieve SocialService data using repository
            var serviceUsers = _serviceUserRepository.GetAll(); // Retrieve ServiceUser data using repository

            var viewModel = new ServiceUserViewModel
            {
                SocialServices = socialServices,
                ServiceUsers = serviceUsers
            };

            return View(viewModel);
        }

        //--------//
        public IActionResult Create()
        {
            var customers = _customerRepository.GetAvailableCustomers();
            ViewBag.CustomerCids = new SelectList(customers, "Cid", "Cid");

            var socialServices = _socialServiceRepository.GetAll();
            ViewBag.Services = socialServices ?? new List<SocialService>();

            return View();
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) && phoneNumber.Length == 11 && phoneNumber.All(char.IsDigit);
        }

        [HttpGet]
        public IActionResult GetCustomerName(int cid)
        {
            // Fetch the customer name based on the provided cid
            var customer = _customerRepository.GetById(cid);

            if (customer != null)
            {
                return Ok(customer.Cname);
            }

            return NotFound();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ServiceUser serviceUser, string[] serviceIds)
        {
            if (serviceIds?.Length > 0)
            {
                serviceUser.ServiceUse = string.Join(",", serviceIds);
                serviceUser.Cname = _customerRepository.GetById(serviceUser.CustomerCid)?.Cname;

                // Check phone number format
                if (!IsValidPhoneNumber(serviceUser.PhoneNumber))
                {
                    ModelState.AddModelError("PhoneNumber", "Phone number must be 11 digits and contain only numbers (0-9).");
                }

                // Check if at least one service is selected
                if (string.IsNullOrEmpty(serviceUser.ServiceUse))
                {
                    ModelState.AddModelError("ServiceUse", "Please select at least one service.");
                }

                if (1==1)
                {
                    _serviceUserRepository.Create(serviceUser);
                    TempData["UserAdded"] = true; // Add success flag to TempData

                    return RedirectToAction("Index");
                }
            }

            var customers = _customerRepository.GetAvailableCustomers();
            ViewBag.CustomerCids = new SelectList(customers, "Cid", "Cid", serviceUser.CustomerCid);

            var socialServices = _socialServiceRepository.GetAll();
            ViewBag.Services = socialServices ?? new List<SocialService>();

            return View(serviceUser);
        }

        //-------//

        public IActionResult Edit(int id)
        {
            var serviceUser = _serviceUserRepository.GetById(id);

            if (serviceUser == null)
            {
                return NotFound();
            }

            var customers = _customerRepository.GetAvailableCustomers();
            ViewBag.CustomerCids = new SelectList(customers, "Cid", "Cid", serviceUser.CustomerCid);

            var socialServices = _socialServiceRepository.GetAll();
            ViewBag.Services = socialServices ?? new List<SocialService>();

            var selectedServices = serviceUser.ServiceUse?.Split(',')?.ToList() ?? new List<string>();
            ViewBag.SelectedServiceIds = selectedServices;
            return View("Edit", serviceUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ServiceUser serviceUser, string[] serviceIds)
        {

            Console.WriteLine("---This is Rifat just check for security validation ---");
            serviceUser.ServiceUse = string.Empty; // Clear the ServiceUse propert

            if (serviceIds != null && serviceIds.Length > 0)
            {


                serviceUser.ServiceUse = string.Join(",", serviceIds);
            }

            _serviceUserRepository.Update(serviceUser);
            TempData["UserUpdated"] = true; // Add success flag to TempData
            return RedirectToAction("Index");

        }

        //-------//

        public IActionResult Delete(int id)
        {
            var serviceUser = _serviceUserRepository.GetById(id);

            if (serviceUser == null)
            {
                return NotFound();
            }

            var viewModel = new ServiceUserViewModel
            {
                SocialServices = _socialServiceRepository.GetAll(),
                ServiceUsers = new List<ServiceUser> { serviceUser }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmDelete(int id, ServiceUserViewModel viewModel)
        {
            _serviceUserRepository.Delete(id);

            viewModel.SocialServices = _socialServiceRepository.GetAll();
            TempData["UserDeleted"] = true; // Add success flag to TempData

            return RedirectToAction("Index");
        }



        private int GetNextServiceUserCid()
        {
            int maxCid = _serviceUserRepository.GetMaxCid();
            return maxCid + 1;
        }



        // excel export//

        public IActionResult ExportToExcel()
        {
            var socialServices = _socialServiceRepository.GetAll(); ; 
            var serviceUsers = _serviceUserRepository.GetAll();

            // Create a list to store the data for export
            var exportData = new List<object[]>();

            // Add the header row
            exportData.Add(new object[] { "Customer Id", "Customer Name", "Phone Number", "Services Use", "Created Date Time", "Updated Date Time" });

            // Add the data rows
            foreach (var serviceUser in serviceUsers)
            {
                var services = new List<string>();
                var serviceIds = serviceUser.ServiceUse?.Split(',') ?? new string[0];
                foreach (var serviceId in serviceIds)
                {
                    if (int.TryParse(serviceId, out var id) && socialServices.FirstOrDefault(s => s.ServiceId == id) is SocialService socialService)
                    {
                        services.Add(socialService.ServiceName);
                    }
                }

                exportData.Add(new object[]
                {
                serviceUser.Cid,
                serviceUser.Cname,
                serviceUser.PhoneNumber,
                string.Join(", ", services),
                serviceUser.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                serviceUser.UpdatedAt != serviceUser.CreatedAt ? serviceUser.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss") : ""
                });
            }

            // Generate the Excel file using EPPlus
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("ServiceUsers");
                worksheet.Cells.LoadFromArrays(exportData);

                using (var headerRange = worksheet.Cells[1, 1, 1, exportData[0].Length])
                {
                    headerRange.Style.Font.Bold = true;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);

                // Set the stream position back to 0 to ensure the file is read from the beginning.
                stream.Position = 0;

                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                var fileName = "ServiceUsers.xlsx";

                return File(stream, contentType, fileName);
            }
        }


    }

}
