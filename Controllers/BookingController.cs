using DMSystemMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMSystemMvc.Controllers
{
    public class BookingController : Controller
    {
        private readonly DeliveryManagementSystemContext _db;
        public BookingController(DeliveryManagementSystemContext db)
        {
            _db = db;
        }

        // GET: BookingController
        public ActionResult BookingList()
        {
            try
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId != null && currentUserId > 0)
                {
                    var userType = HttpContext.Session.GetString("UserType");
                    List<OrderDetail> booking = new List<OrderDetail>();
                    if (userType == "Customer")
                        booking = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).Where(w => w.CustomerId == currentUserId && (w.TimeOfPickup >= DateTime.Now || w.DeliveryDate >= DateTime.Now)).ToList();
                    else if (userType == "Executive")
                        booking = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).Where(w => w.ExecutiveId == currentUserId && (w.TimeOfPickup >= DateTime.Now || w.DeliveryDate >= DateTime.Now)).ToList();
                    return View(booking);
                }
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId != null && currentUserId > 0)
                {
                    var orders = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).SingleOrDefault(s => s.OrderId == id);
                    return View(orders);
                }
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            try
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId != null && currentUserId > 0)
                {
                    var userType = HttpContext.Session.GetString("UserType");
                    ViewBag.Customers = _db.Registrations.Where(w => w.RegistrationType == "Customer" && (w.Id == currentUserId && userType == "Customer")).Select(
                        s => new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList();
                    ViewBag.Executives = _db.Registrations.Where(w => w.RegistrationType == "Executive" && ((w.Id == currentUserId && userType == "Executive") || currentUserId != 0)).Select(
                        s => new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList();

                    return View();
                }
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderDetail orderDetail)
        {
            try
            {

                _db.OrderDetails.Add(orderDetail);
                _db.SaveChanges();
                return RedirectToAction("BookingList");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId != null && currentUserId > 0)
                {
                    var userType = HttpContext.Session.GetString("UserType");

                    ViewBag.Customers = _db.Registrations.Where(w => w.RegistrationType == "Customer" && (w.Id == currentUserId && userType == "Customer")).Select(
                      s => new SelectListItem
                      {
                          Value = s.Id.ToString(),
                          Text = s.Name
                      }).ToList();
                    ViewBag.Executives = _db.Registrations.Where(w => w.RegistrationType == "Executive" && ((w.Id == currentUserId && userType == "Executive") || currentUserId != 0)).Select(
                        s => new SelectListItem
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList();
                    var order = _db.OrderDetails.Find(id);
                    return View(order);
                }
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDetail orderDetail)
        {
            try
            {
                _db.OrderDetails.Update(orderDetail);
                _db.SaveChanges();
                return RedirectToAction("BookingList");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId != null && currentUserId > 0)
                {
                    var booking = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).SingleOrDefault(s => s.OrderId == id);
                    return View(booking);
                }
                return RedirectToAction("Login", "Registration");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(OrderDetail orderDetail)
        {
            try { 

            _db.OrderDetails.Remove(orderDetail);
            _db.SaveChanges();
            return RedirectToAction("BookingList");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
       
    }
}
