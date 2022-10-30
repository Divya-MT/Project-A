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
        public IActionResult Registration([FromBody] Registration registration)
        {
            _db.Registrations.Add(registration);
            _db.SaveChanges();
            return Ok(registration);
        }

        [HttpPost]
        public IActionResult Login([FromBody] Login login)
        {
            //linq - language integrety query
            var result = (from i in _db.Registrations
                          where i.Name == login.Name
                          && i.RegistrationType == login.RegistrationType
                          && i.Password == login.Password
                          select i).SingleOrDefault();
            if (result != null)
            {

                return Ok(result);
            }
            return NotFound();
        }
    }
}
