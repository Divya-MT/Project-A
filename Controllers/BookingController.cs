using DMSystemMvc.Models;
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
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");
            List<OrderDetail> booking=new List<OrderDetail>();
            if(userType=="Customer")
            booking = _db.OrderDetails.Include(i=>i.Customer).Include(i=>i.Executive).Where(w=>w.CustomerId==currentUserId && (w.TimeOfPickup>=DateTime.Now || w.DeliveryDate>=DateTime.Now)).ToList();
            else if(userType == "Executive")
                booking = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).Where(w => w.ExecutiveId == currentUserId && (w.TimeOfPickup >= DateTime.Now || w.DeliveryDate >= DateTime.Now)).ToList();
            return View(booking);
        }

        // GET: BookingController/Details/5
        public ActionResult Details(int id)

        {
            var orders = _db.OrderDetails.Include(i=>i.Customer).Include(i=>i.Executive).SingleOrDefault(s=>s.OrderId==id);


            return View(orders);
        }

        // GET: BookingController/Create
        public ActionResult Create()
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
            var userType = HttpContext.Session.GetString("UserType");
            ViewBag.Customers = _db.Registrations.Where(w => w.RegistrationType == "Customer" && (w.Id== currentUserId && userType== "Customer")).Select(
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

        // POST: BookingController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderDetail orderDetail)
        {

            _db.OrderDetails.Add( orderDetail);
            _db.SaveChanges();
            return RedirectToAction("BookingList");
            
        }

        // GET: BookingController/Edit/5
        public ActionResult Edit(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");
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
            var order=_db.OrderDetails.Find(id);
            return View(order);
        }

        // POST: BookingController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderDetail orderDetail)
        {
            _db.OrderDetails.Update(orderDetail);
            _db.SaveChanges();
            return RedirectToAction("BookingList");


            //try
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: BookingController/Delete/5
        public ActionResult Delete(int id)
        {
            var booking = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).SingleOrDefault(s => s.OrderId == id);
            return View(booking);
        }

        // POST: BookingController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(OrderDetail orderDetail)
        {

            _db.OrderDetails.Remove(orderDetail);
            _db.SaveChanges();
            return RedirectToAction("BookingList");


            //try
            //{
            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}

        }
       
    }
}
