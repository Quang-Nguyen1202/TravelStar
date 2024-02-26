using Microsoft.AspNetCore.Mvc;
using TravelStar.Business.Implements;
using TravelStar.Business.Interfaces;
using TravelStar.Entities;

namespace TravelStar.Site.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomBo _roomBo;

        public RoomController(
            IRoomBo roomBo )
        {
            _roomBo = roomBo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("/removeroom")]
        public IActionResult RemoveRoom(int id)
        {
            using (var unitOfWork = _roomBo.NewDbContext())
            {
                Room roomEntitty = _roomBo.GetById(unitOfWork, id);

                try
                {
                    _roomBo.DeleteEntity(roomEntitty, unitOfWork);
                    return Json(true);
                }
                catch (Exception ex)
                {
                    return Json(false);
                }
            }
        }
    }
}
