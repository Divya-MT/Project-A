using DMSystemMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Plugins;

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
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registration(Registration registration)
        {
            try
            {
                ////Without API call

                //_db.Registrations.Add(registration);
                //_db.SaveChanges();
                //return RedirectToAction("Login");

                //With API call
                using (var client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(registration), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("https://localhost:7098/api/Registration/Registration", content))
                    {
                        string apiresponse = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Registration>(apiresponse);
                        if (result != null)
                        {
                            return RedirectToAction("Login", "Registration");
                        }
                        else
                            throw new Exception("Registration was not successful. Kindly Register Again.");
                    }
                }

                
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        public IActionResult Login()
        {
            try
            {
                return View();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                //linq - language integrety query
                // Login Validation 

                ////without API Call

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

                //using API Call

                using (var client = new HttpClient())
                {
                    StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                    using (var response = await client.PostAsync("https://localhost:7098/api/Registration/Login", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string apiresponse = await response.Content.ReadAsStringAsync();
                            var registration = JsonConvert.DeserializeObject<Registration>(apiresponse);
                            if (registration != null)
                            {
                                HttpContext.Session.SetInt32("UserId", registration.Id);
                                HttpContext.Session.SetString("UserName", registration.Name);
                                HttpContext.Session.SetString("UserType", registration.RegistrationType);
                                return RedirectToAction("BookingList", "Booking");
                            }
                            else
                                throw new Exception("Login Credentials are Not Found");
                        }
                        else
                            throw new Exception("Login Credentials are Not Found");
                    }
                }


            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
