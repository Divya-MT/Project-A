using DeliveryManagementSystemApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly DeliveryManagementSystemContext _db;
        public RegistrationController(DeliveryManagementSystemContext db)
        {
            _db = db;
        }


        [HttpPost]
        [Route("Registration")]
        public IActionResult Registration([FromBody] Registration registration)
        {
            try { 
            _db.Registrations.Add(registration);
            _db.SaveChanges();
            return Ok(registration);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Login login)
        {
            try { 
            //linq - language integrety query
            var result = (from i in _db.Registrations
                          where i.Name == login.Name
                          && i.RegistrationType == login.RegistrationType
                          && i.Password == login.Password
                          select i).SingleOrDefault();
          

            // return Ok(result);
            if (result != null)
            {

                return Ok(result);
            }
            else
            {
                return NotFound();
            }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            } 

        }
    }
}