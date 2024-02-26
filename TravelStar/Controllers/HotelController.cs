using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Model;
using TravelStar.Site.Models.Home;
using TravelStar.Site.Models.Hotel;
using TravelStar.Site.Models.Room;

namespace TravelStar.Site.Controllers
{
    public class HotelController : Controller
    {
        private readonly ICityBo _cityBo;
        private readonly IHotelBo _hotelBo;
        private readonly IRoomBo _roomBo;
        private readonly ICustomerBo _CustomerBo;
        private readonly IBookingBo _BookingBo;
        private readonly IEmailService _emailService;
        private readonly UserManager<AppUser> _userManager;
       
        public HotelController(ICityBo cityBo,
            IHotelBo hotelBo,
            IRoomBo roomBo,
            ICustomerBo customerBo,
            IBookingBo bookingBo,
            IEmailService emailService,
            UserManager<AppUser> userManager)
        {
            _cityBo = cityBo;
            _hotelBo = hotelBo;
            _roomBo = roomBo;
            _CustomerBo = customerBo;
            _BookingBo = bookingBo;
            _emailService = emailService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            return View(currentUser);
        }

        [Authorize(Roles = "Admin, SuperAdmin")]
        [HttpPost("createhotel")]
        public IActionResult CreateHotel(CreateHotelModel model)
        {
            try
            {
                using (var unitofwork = _hotelBo.NewDbContext())
                {
                    Hotel entity = new()
                    {
                        Name = model.Name,
                        Phone = model.Phone,
                        Address = model.Address,
                        WardId = model.WardId,
                        UserId = model.UserId
                    };
                    Hotel? hotelNew = _hotelBo.InsertEntity(entity, unitofwork);

                    if (hotelNew is not null)
                    {
                        foreach (var image in model.Images)
                        {
                            if (image.Length > 0)
                            {
                                //Get url To Save
                                string uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels/" + hotelNew.Id);
                                Directory.CreateDirectory(uploadFolderPath);
                                string SavePath = Path.Combine(uploadFolderPath, image.FileName);
                                using (var stream = new FileStream(SavePath, FileMode.Create))
                                {
                                    image.CopyTo(stream);
                                }
                            }
                        }
                    }
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelListAsync()
        {
            AppUser currentUser = await _userManager.GetUserAsync(HttpContext.User);
            using (var unitOfWork = _hotelBo.NewDbContext())
            {
                IQueryable<Hotel> hotelList = _hotelBo.GetEntities(unitOfWork)
                    //.Where(x => x.UserId == currentUser.Id)
                    .Include(x => x.User)
                    .Include(x => x.Ward)
                        .ThenInclude(x => x!.District)
                            .ThenInclude(x => x!.City);

                if (!User.IsInRole("SuperAdmin"))
                {
                    hotelList = hotelList.Where(x => x.UserId == currentUser.Id);
                }

                List<HotelViewModel> hotelListViewModel = new List<HotelViewModel>();
                foreach (Hotel item in hotelList)
                {
                    HotelViewModel hotelViewModel = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Address = item.Address,
                        Phone = item.Phone,
                        WardId = item.WardId,
                        Ward = item.Ward!.Name,
                        District = item.Ward.District!.Name,
                        City = item.Ward.District.City!.Name,
                        EmailOwner = item.User is not null ? item.User.Email : string.Empty,
                        IsSuperAdmin = User.IsInRole("SuperAdmin")
                    };

                    hotelListViewModel.Add(hotelViewModel);
                }

                return Json(new { data = hotelListViewModel });
            }
        }

        [HttpPost]
        public IActionResult SearchHotel(SearchHotelModel model)
        {
            using (var unitOfWork = _hotelBo.NewDbContext())
            {
                //Lấy Room Id đã được booking
                List<int> roomIds = _BookingBo.GetEntities(unitOfWork)
                    .Where(x => x.CancelDate == null && ((x.CheckInDate.Date <= model.CheckInDate!.Value.Date && x.CheckOutDate.Date >= model.CheckInDate!.Value.Date) ||
                    (x.CheckInDate.Date <= model.CheckOutDate!.Value.Date && x.CheckOutDate.Date >= model.CheckOutDate!.Value.Date)))
                    .Select(x => x.RoomId).ToList();

                List<Hotel> hotelList = _hotelBo.GetEntities(unitOfWork)
                    .Include(x => x.Rooms!.Where(x => !roomIds.Contains(x.Id)))
                    .Include(x => x.Ward)
                        .ThenInclude(x => x!.District)
                            .ThenInclude(x => x!.City)
                    .Where(x => x.Name!.Contains(model.City!) || x.Ward!.District!.City!.Name!.Contains(model.City!)).ToList();

                //Lấy những hotel còn room
                hotelList = hotelList.Where(x => x.Rooms!.Count() > 0).ToList();

                List<HotelViewModel> hotelListViewModel = new List<HotelViewModel>();
                foreach (Hotel item in hotelList)
                {
                    HotelViewModel hotelViewModel = new()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Address = item.Address,
                        Phone = item.Phone,
                        WardId = item.WardId,
                        Ward = item.Ward!.Name,
                        District = item.Ward.District!.Name,
                        City = item.Ward.District.City!.Name,
                        MinPrice = item.Rooms!.MinBy(x => x.Price)!.Price
                    };

                    hotelListViewModel.Add(hotelViewModel);
                }

                SearchViewModel modelSearch = new()
                {
                    HotelList = hotelListViewModel,
                };

                return PartialView("~/Views/Home/_HotelList.cshtml", modelSearch);
            }
        }

        [HttpGet("city")]
        public IActionResult GetCity()
        {
            using (var unitOfWork = _cityBo.NewDbContext())
            {
                List<City> cityList = _cityBo.GetEntities(unitOfWork).Include(x => x.Districts!).ThenInclude(x => x.Wards).ToList();

                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles,
                    WriteIndented = true
                };

                return Json(new { cityList = cityList }, options);
            }
        }

        [HttpPost("remove")]
        public IActionResult DeleteHotel(int id)
        {
            using (var unitOfWork = _hotelBo.NewDbContext())
            {
                Hotel hotelEntitty = _hotelBo.GetById(unitOfWork, id);

                try
                {
                    _hotelBo.DeleteEntity(hotelEntitty, unitOfWork);
                    return Json(true);
                }
                catch (Exception ex)
                {
                    return Json(false);
                }
            }
        }

        [HttpGet("/hotel/rooms")]
        public IActionResult GetRoomList(int hotelId, DateTime checkInDate, DateTime checkOutDate)
        {
            using (var unitOfWork = _hotelBo.NewDbContext())
            {
                List<int> roomIds = _BookingBo.GetEntities(unitOfWork)
                    .Where(x => (x.CheckInDate.Date > checkInDate.Date && x.CheckInDate.Date > checkOutDate.Date) || (x.CheckOutDate.Date > checkInDate.Date && x.CheckOutDate.Date > checkOutDate.Date))
                    .Select(x => x.RoomId).ToList();

                Hotel? hotel = _hotelBo.GetEntities(unitOfWork, x => x.Id == hotelId)
                    .Include(x => x.Rooms)
                    .Include(x => x.Ward)
                        .ThenInclude(x => x!.District)
                            .ThenInclude(x => x!.City).FirstOrDefault();

                HotelModel hotelModel = new();
                if (hotel is not null)
                {
                    hotelModel.Id = hotel.Id;
                    hotelModel.Name = hotel.Name;
                    hotelModel.Address = hotel.Address;
                    hotelModel.Phone = hotel.Phone;
                    hotelModel.WardId = hotel.WardId;
                    hotelModel.Ward = hotel.Ward!.Name;
                    hotelModel.District = hotel.Ward.District!.Name;
                    hotelModel.City = hotel.Ward.District.City!.Name;
                    hotelModel.CheckInDate = checkInDate;
                    hotelModel.CheckOutDate = checkOutDate;

                    string hotelImageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/hotels/" + hotelModel.Id);
                    List<string> hotelFilePaths = new List<string>();

                    if (Directory.Exists(hotelImageFolderPath))
                    {
                        hotelFilePaths = Directory.GetFiles(hotelImageFolderPath).ToList();

                    }

                    foreach (string filePath in hotelFilePaths)
                    {
                        hotelModel.ImageUrls.Add("/images/hotels/" + hotelModel.Id + "/" + Path.GetFileName(filePath));
                    }

                    if (hotel.Rooms is not null)
                    {
                        foreach (Room room in hotel.Rooms)
                        {
                            RoomModel roomModel = new RoomModel()
                            {
                                Id = room.Id,
                                Name = room.Name,
                                Description = room.Description,
                                Type = room.Type,
                                Beds = room.Beds,
                                HasBreakfast = room.HasBreakfast,
                                Price = room.Price,
                            };
                            string imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms/" + room.Id);
                            List<string> filePaths = new List<string>();

                            if (Directory.Exists(imageFolderPath))
                            {
                                filePaths = Directory.GetFiles(imageFolderPath).ToList();

                            }

                            foreach (string filePath in filePaths)
                            {
                                roomModel.ImageUrls.Add("/images/rooms/" + room.Id + "/" + Path.GetFileName(filePath));
                            }

                            hotelModel.Rooms.Add(roomModel);
                        }
                    }
                }

                return View("HotelRooms", hotelModel);
            }
        }

        [HttpPost("createroom")]
        public IActionResult CreateRoom([FromForm] CreateRoomModel model)
        {
            try
            {
                using (var unitofwork = _roomBo.NewDbContext())
                {
                    Room entity = new()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Type = model.Type,
                        Beds = model.Beds,
                        HotelId = model.HotelId,
                        Price = model.Price,
                    };
                    Room? roomNew = _roomBo.InsertEntity(entity, unitofwork);

                    if (roomNew is not null)
                    {
                        foreach (var image in model.Images)
                        {
                            if (image.Length > 0)
                            {
                                //Get url To Save
                                string uploadFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms/" + roomNew.Id);
                                Directory.CreateDirectory(uploadFolderPath);
                                string SavePath = Path.Combine(uploadFolderPath, image.FileName);
                                using (var stream = new FileStream(SavePath, FileMode.Create))
                                {
                                    image.CopyTo(stream);
                                }
                            }
                        }
                    }
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost("createbooking")]
        public IActionResult CreateBooking(BookingModel model)
        {
            try
            {
                Random RandNum = new Random();
                string ConfirmCode = RandNum.Next(100000, 999999).ToString();

                using (var unitofwork = _BookingBo.NewDbContext())
                {
                    Customer? customerEntitty = _CustomerBo.GetEntities(unitofwork, x => x.Email!.Equals(model.CustomerEmail)).FirstOrDefault();
                    if (customerEntitty is null)
                    {
                        customerEntitty = new()
                        {
                            Name = model.CustomerEmail,
                            Email = model.CustomerEmail,
                            Phone = model.CustomerPhone,
                        };

                        customerEntitty = _CustomerBo.InsertEntity(customerEntitty, unitofwork);
                    }

                    Booking bookingEntity = new()
                    {
                        CheckInDate = model.CheckInDate,
                        CheckOutDate = model.CheckOutDate,
                        BookingDate = DateTime.Now,
                        TotalPrice = model.TotalPrice,
                        CustomerId = customerEntitty.Id,
                        RoomId = model.RoomId,
                        ConfirmCode = ConfirmCode
                    };

                    bookingEntity = _BookingBo.InsertEntity(bookingEntity, unitofwork);

                    _emailService.SendConfirmBookingEmailAsync(customerEntitty.Email!, "TravelStar Confirm Booking", ConfirmCode);
                    return Json(new { status = true, bookingId = bookingEntity.Id, customerEmail = customerEntitty.Email });
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }

        [HttpPost("confirmbooking")]
        public IActionResult ConfirmBooking(int bookingId, string confirmCode, string customerEmail)
        {
            try
            {
                using (var unitofwork = _BookingBo.NewDbContext())
                {
                    Booking bookingEntity = _BookingBo.GetById(unitofwork, bookingId);
                    if (bookingEntity is not null && bookingEntity.ConfirmCode!.Equals(confirmCode))
                    {
                        bookingEntity.IsConfirm = true;
                        _BookingBo.UpdateEntity(bookingEntity, bookingEntity.Id, unitofwork);
                        _emailService.SendInfoBookingSuccessEmailAsync(customerEmail, "TravelStar info booking successfully");
                        return Json(true);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(false);
            }
        }
    }
}