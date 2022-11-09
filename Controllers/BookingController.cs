using DMSystemMvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DMSystemMvc.Controllers
{
    /// <summary>
    /// To manage Delivery booking process of Customer and Executives
    /// </summary>
    public class BookingController : Controller
    {
        //Db Context Declaration
        private readonly DeliveryManagementSystemContext _db;

        //Injecting the DB context in BookingController's Constructor
        public BookingController(DeliveryManagementSystemContext db)
        {
            //Mapping both object of injected and declared
            _db = db;
        }

        /// <summary>
        /// To View Booking
        /// </summary>
        /// <returns></returns>
        // GET: BookingController
        public ActionResult BookingList()
        {
            try
            {
                //To get the UserId from the Session which was assigned after the login
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                //To validate the UserId is valid one
                if (currentUserId != null && currentUserId > 0)//if Valid
                {
                    //To get the UserType from the Session which was assigned after the login
                    var userType = HttpContext.Session.GetString("UserType");
                    List<OrderDetail> booking = new List<OrderDetail>();

                    if (userType == "Customer")
                        //Getting the Today's and Future Bookings which includes Customer and Excutive details for Current Customer
                        booking = _db.OrderDetails.Include(i => i.Customer).Include(i => i.Executive).Where(w => w.CustomerId == currentUserId && (w.TimeOfPickup >= DateTime.Now || w.DeliveryDate >= DateTime.Now)).ToList();
                    else if (userType == "Executive")
                        //Getting the Today's and Future Bookings which includes Customer and Excutive details for Current Executive
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

        /// <summary>
        /// To show the details of particular booked order
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To view the Add Booking page
        /// </summary>
        /// <returns></returns>
        // GET: BookingController/Create
        public ActionResult Create()
        {
            try
            {
                var currentUserId = HttpContext.Session.GetInt32("UserId");
                if (currentUserId != null && currentUserId > 0)
                {
                    var userType = HttpContext.Session.GetString("UserType");

                    //To pass the Customer Data  to the View of Add booking
                    ViewBag.Customers = _db.Registrations
                        .Where(w => w.RegistrationType == "Customer" && (w.Id == currentUserId && userType == "Customer"))
                        .Select(
                        s => new SelectListItem //in build model for dropdown control 
                        {
                            Value = s.Id.ToString(),
                            Text = s.Name
                        }).ToList();

                    //To pass the Executive Data  to the View of Add booking
                    ViewBag.Executives = _db.Registrations.Where(w => w.RegistrationType == "Executive" 
                    && ((w.Id == currentUserId && userType == "Executive") || currentUserId != 0)).Select(
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

        /// <summary>
        /// Add booking Submit
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
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
        /// <summary>
        /// To Show Edit Order detail view page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Edit Submit
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
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

        /// <summary>
        /// To show Cancellation Detail page
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: BookingController/ConcellationDetails/5
        public ActionResult ConcellationDetails(int id)
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

        /// <summary>
        /// Cancel Submit
        /// </summary>
        /// <param name="orderDetail"></param>
        /// <returns></returns>
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
