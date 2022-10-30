using DMSystemMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DMSystemMvc.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly DeliveryManagementSystemContext _db;
        public RegistrationController(DeliveryManagementSystemContext db)
        {
            _db = db;
            
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registration(Registration registration)
        {
            _db.Registrations.Add(registration);
            _db.SaveChanges();
            return RedirectToAction("Login");
        }

        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            //linq - language integrety query
            // Login Validation 


            //var result = (from i in _db.Registrations
            //              where i.Name == login.Name
            //              && i.RegistrationType == login.RegistrationType
            //              && i.Password == login.Password
            //              select i).SingleOrDefault();



            //if (result != null)
            //{

            //    HttpContext.Session.SetInt32("UserId", result.Id);
            //    HttpContext.Session.SetString("UserName", result.Name);
            //    HttpContext.Session.SetString("UserType", result.RegistrationType);

            //    return RedirectToAction("BookingList", "Booking");
            //}
            //return null;

            using (var client = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                using (var response = await client.PostAsync("https://localhost:7098/api/Registration", content))
                {
                    string apiresponse = await response.Content.ReadAsStringAsync();
                    var registration = JsonConvert.DeserializeObject<Registration>(apiresponse);
                    if(registration!=null)
                    {
                        HttpContext.Session.SetInt32("UserId", registration.Id);
                        HttpContext.Session.SetString("UserName", registration.Name);
                        HttpContext.Session.SetString("UserType", registration.RegistrationType);
                        return RedirectToAction("BookingList", "Booking");
                    }
                }
            }
            
            return RedirectToAction("Login", "Registration");
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Registration");
        }
    }
}
