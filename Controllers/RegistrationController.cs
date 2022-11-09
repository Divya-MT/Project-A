using DMSystemMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Http;
using NuGet.Protocol.Plugins;

namespace DMSystemMvc.Controllers
{
    /// <summary>
    /// Registration and Login/Logout get and Post
    /// </summary>
    public class RegistrationController : Controller
    {
        //private readonly DeliveryManagementSystemContext _db;
        //public RegistrationController(DeliveryManagementSystemContext db)
        //{
        //    _db = db;

        //}


        /// <summary>
        /// To show the Registration View Page 
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// To submit the Registration 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To show the Login View Page
        /// </summary>
        /// <returns></returns>
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
        /// To Submit the Login credentials
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
                    //Convert and serialize Login object into Json object
                    StringContent content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
                   //Sent the json converted object to the Post method Login in API application and get the Http response
                    using (var response = await client.PostAsync("https://localhost:7098/api/Registration/Login", content))
                    {
                        //Validating the response is successfull or not
                        if (response.IsSuccessStatusCode) //If true
                        {
                            //Getting the required registration response content
                            string apiresponse = await response.Content.ReadAsStringAsync();
                            //Converting back to Registration object from json object
                            var registration = JsonConvert.DeserializeObject<Registration>(apiresponse);
                            //If the registration is not null
                            if (registration != null)
                            {
                                //Setting the UserId, UserName and UserType values in Session 
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

        /// <summary>
        /// To Logout
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {
            try
            {
                //To clear the Session after the Logout
                HttpContext.Session.Clear();
                //Return back to login after the Logout
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
