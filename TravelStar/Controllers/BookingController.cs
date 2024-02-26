using BraintreeHttp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPal.Core;
using PayPal.v1.Payments;
using System.Net;
using TravelStar.Business.Implements;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Repositories.UnitOfWork;
using HttpResponse = BraintreeHttp.HttpResponse;

namespace TravelStar.Site.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingBo _BookingBo;
        private readonly string _clientId;
        private readonly string _secretKey;

        public BookingController(
            IBookingBo bookingBo,
            IConfiguration config)
        {
            _BookingBo = bookingBo;
            _clientId = config["Paypal:ClientId"];
            _secretKey = config["Paypal:SecretKey"];
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/booking/confirmcustomer")]
        public IActionResult GetBookingByEmail(string email)
        {
            using (var unitOfWork = _BookingBo.NewDbContext())
            {
                if (User.IsInRole("SuperAdmin"))
                {
                    List<Booking> allbookings = _BookingBo.GetEntities(unitOfWork)
                .Include(x => x.Customer)
                .Include(x => x.Room)
                    .ThenInclude(x => x!.Hotel)
                    .ThenInclude(x => x!.Ward)
                    .ThenInclude(x => x!.District)
                    .ThenInclude(x => x!.City).ToList();
                
                    return PartialView("~/Views/Booking/_BookingList.cshtml", allbookings);
                }
                else
                {
                    List<Booking> bookings = _BookingBo.GetEntities(unitOfWork)
                .Include(x => x.Customer)
                .Include(x => x.Room)
                    .ThenInclude(x => x!.Hotel)
                    .ThenInclude(x => x!.Ward)
                    .ThenInclude(x => x!.District)
                    .ThenInclude(x => x!.City)
                .Where(x => x.Customer!.Email!.Equals(email)).ToList();

                    return PartialView("~/Views/Booking/_BookingList.cshtml", bookings);
                }

            }
        }


        [HttpPost("cancelbooking")]
        public IActionResult CancelBooking(int id)
        {
            using (var unitOfWork = _BookingBo.NewDbContext())
            {
                Booking bookingEntitty = _BookingBo.GetById(unitOfWork, id);

                try
                {
                    bookingEntitty.CancelDate = DateTime.Now;
                    _BookingBo.UpdateEntity(bookingEntitty, id, unitOfWork);
                    return Json(true);
                }
                catch (Exception ex)
                {
                    return Json(false);
                }
            }
        }

        public async Task<IActionResult> PaypalCheckout(int bookingId)
        {
            using (var unitOfWork = _BookingBo.NewDbContext())
            {
                Booking? booking = _BookingBo.GetEntities(unitOfWork)
                    .Include(x => x.Customer).Where(x => x.Id == bookingId).FirstOrDefault();
                if (booking is null || booking.TotalPrice == 0)
                {
                    return Redirect("/Booking/CheckoutFail");
                }

                SandboxEnvironment environment = new SandboxEnvironment(_clientId, _secretKey);
                PayPalHttpClient client = new PayPalHttpClient(environment);

                #region Create Paypal Order
                ItemList itemList = new ItemList()
                {
                    Items = new List<Item>()
                };

                long total = booking.TotalPrice;

                itemList.Items.Add(new Item()
                {
                    Name = booking.Customer!.Name,
                    Currency = "USD",
                    Price = total.ToString(),
                    Quantity = "1",
                    Sku = "bookingId_" + booking.Id,
                    Tax = "0"
                });
                #endregion

                long paypalOrderId = DateTime.Now.Ticks;
                var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
                var payment = new Payment()
                {
                    Intent = "sale",
                    Transactions = new List<Transaction>()
                    {
                        new Transaction()
                        {
                            Amount = new Amount()
                            {
                                Total = total.ToString(),
                                Currency = "USD",
                                Details = new AmountDetails
                                {
                                    Tax = "0",
                                    Shipping = "0",
                                    Subtotal = total.ToString()
                                }
                            },
                            ItemList = itemList,
                            Description = $"Invoice #{paypalOrderId}",
                            InvoiceNumber = paypalOrderId.ToString()
                        }
                    },
                    RedirectUrls = new RedirectUrls()
                    {
                        CancelUrl = $"{hostname}/Booking/CheckoutFail",
                        ReturnUrl = $"{hostname}/Booking/CheckoutSuccess?bookingId={booking.Id}&email={booking.Customer.Email}"
                    },
                    Payer = new Payer()
                    {
                        PaymentMethod = "paypal"
                    }
                };

                PaymentCreateRequest request = new PaymentCreateRequest();
                request.RequestBody(payment);

                try
                {
                    HttpResponse response = await client.Execute(request);
                    HttpStatusCode statusCode = response.StatusCode;
                    Payment result = response.Result<Payment>();

                    var links = result.Links.GetEnumerator();
                    string paypalRedirectUrl = null!;
                    while (links.MoveNext())
                    {
                        LinkDescriptionObject lnk = links.Current;
                        if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.Href;
                        }
                    }

                    return Redirect(paypalRedirectUrl);
                }
                catch (HttpException httpException)
                {
                    var statusCode = httpException.StatusCode;
                    var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                    //Process when Checkout with Paypal fails
                    return Redirect("/Booking/CheckoutFail");
                }
            }
        }

        public IActionResult CheckoutFail()
        {
            return View();
        }

        public IActionResult CheckoutSuccess(int bookingId, string email)
        {
            using (var unitOfWork = _BookingBo.NewDbContext())
            {
                Booking bookingEntitty = _BookingBo.GetById(unitOfWork, bookingId);

                try
                {
                    bookingEntitty.PaymentDate = DateTime.Now;
                    _BookingBo.UpdateEntity(bookingEntitty, bookingId, unitOfWork);
                }
                catch (Exception ex)
                {
                    return Redirect("/Booking/CheckoutFail");
                }
            }
            return Redirect("/Booking/Index");
        }
    }
}
