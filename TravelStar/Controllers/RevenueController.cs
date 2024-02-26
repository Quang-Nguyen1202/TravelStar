using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2013.Word;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using TravelStar.Business.Implements;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;
using TravelStar.Site.Models.Hotel;
using TravelStar.Site.Models.Revenue;

namespace TravelStar.Site.Controllers
{
    public class RevenueController : Controller
    {
        private readonly IBookingBo _BookingBo;

        public RevenueController(
            IBookingBo bookingBo)
        {
            _BookingBo = bookingBo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetBookingList(DateTime? fromDate = null!, DateTime? toDate = null!)
        {
            using (var unitOfWork = _BookingBo.NewDbContext())
            {
                IQueryable<Booking> BookingList = _BookingBo.GetEntities(unitOfWork)
                       .Include(x => x.Customer)
                       .Include(x => x.Room)
                       .ThenInclude(x => x!.Hotel)
                       .Where(x => x.PaymentDate.HasValue);

                if (fromDate is not null)
                {
                    BookingList = BookingList.Where(x => x.CheckInDate.Date >= fromDate.Value.Date);
                }

                if (toDate is not null)
                {
                    BookingList = BookingList.Where(x => x.CheckInDate.Date <= toDate.Value.Date);
                }

                List<BookingViewModel> bookingListViewModel = new List<BookingViewModel>();
                foreach (Booking item in BookingList.OrderBy(x => x.CheckInDate.Date).ToList())
                {
                    BookingViewModel bookingViewModel = new()
                    {
                        Id = item.Id,
                        CheckInDate = item.CheckInDate.Day.ToString() + "/" + item.CheckInDate.Month.ToString() + "/" + item.CheckInDate.Year.ToString(),
                        CheckOutDate = item.CheckOutDate.Day.ToString() + "/" + item.CheckOutDate.Month.ToString() + "/" + item.CheckOutDate.Year.ToString(),
                        RoomName = item.Room is not null && !string.IsNullOrEmpty(item.Room.Name) ? item.Room.Name : string.Empty,
                        CustomerEmail = item.Customer is not null && !string.IsNullOrEmpty(item.Customer.Email) ? item.Customer.Email : string.Empty,
                        TotalPrice = item.TotalPrice,
                        HotelName = item.Room!.Hotel!.Name,
                    };

                    bookingListViewModel.Add(bookingViewModel);
                }

                return Json(new { data = bookingListViewModel });
            }
        }

        [HttpGet]
        public Task<FileResult> ExportRevenueInExcel(DateTime? fromDate = null!, DateTime? toDate = null!)
        {
            using (var unitOfWork = _BookingBo.NewDbContext())
            {
                IQueryable<Booking> bookings = _BookingBo.GetEntities(unitOfWork)
                       .Include(x => x.Customer)
                       .Include(x => x.Room)
                       .ThenInclude(x => x!.Hotel)
                       .Where(x => x.PaymentDate.HasValue);


                if (fromDate is not null)
                {
                    bookings = bookings.Where(x => x.CheckInDate.Date >= fromDate.Value.Date);
                }

                if (toDate is not null)
                {
                    bookings = bookings.Where(x => x.CheckInDate.Date <= toDate.Value.Date);
                }

                string fileName = "Revenue.xlsx";
                return Task.FromResult(GenerateExcel(fileName, bookings.ToList()));
            }    
            
        }

        private FileResult GenerateExcel(string fileName, List<Booking> bookings)
        {
            DataTable dataTable = new DataTable("revenue");
            dataTable.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("Id"),
                new DataColumn("CheckInDate"),
                new DataColumn("CheckOutDate"),
                new DataColumn("RoomName"),
                new DataColumn("CustomerEmail"),
                new DataColumn("ToTalPrice"),
                new DataColumn("HotelName")
            });

            foreach (Booking booking in bookings)
            {
                dataTable.Rows.Add(booking.Id,
                                   booking.CheckInDate,
                                   booking.CheckOutDate,
                                   booking.Room!.Name,
                                   booking.Customer!.Email,
                                   booking.TotalPrice,
                                   booking.Room!.Hotel!.Name);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);

                    return File(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    fileName);
                }
            }
        }
    }
}
