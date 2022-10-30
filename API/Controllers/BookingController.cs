using DeliveryManagementSystemApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.OleDb;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly DeliveryManagementSystemContext _db;

        public BookingController(DeliveryManagementSystemContext db)
        {
            _db = db;
        }


        // GET: api/<BookingController>
        [Route("GetOrderList")]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_db.OrderDetails.ToList());
        }

        // GET api/<BookingController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok(_db.OrderDetails.Find(id));
        }

        // POST api/<BookingController>
        [HttpPost]
        public IActionResult Post([FromBody] OrderDetail orderDetail)
        {
            _db.OrderDetails.Add(orderDetail);
            _db.SaveChanges();
            return Ok(orderDetail);
        }

        // PUT api/<BookingController>/5
        [HttpPut]
        public IActionResult Put(OrderDetail orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
            _db.SaveChanges();
            return Ok(orderDetail);
        }

        // DELETE api/<BookingController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var orderDetail = _db.OrderDetails.Find(id);
            _db.OrderDetails.Remove(orderDetail);
            _db.SaveChanges();
            return Ok(orderDetail);
        }
    }
}
