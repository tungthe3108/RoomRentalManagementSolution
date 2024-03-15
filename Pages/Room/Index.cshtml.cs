using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.Room
{
    public class IndexModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public IndexModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
            SelectedRooms = new List<int>();
        }
        public List<int> SelectedRooms { get; set; }
        public IList<RoomRentalManagementSolution.Models.Room> Room { get;set; } = default!;
        public RoomRentalManagementSolution.Models.Room RoomSearch { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string RoomId, string RoomName, string FloorId, string NumOfPersonId, 
            string AreaId, string PriceId, string RoomNameOrder, string PriceOrder, string FloorOrder)
        {
            string userName = HttpContext.Session.GetString("username");
            if (userName == null)
            {
                return RedirectToPage("/Login/Index");
            }
            ViewData["RoomIdraw"] = RoomId;
            ViewData["RoomNameraw"] = RoomName;
            ViewData["FloorIdraw"] = FloorId;
            ViewData["NumOfPersonIdraw"] = NumOfPersonId;
            ViewData["PriceIdraw"] = PriceId;
            ViewData["RoomNameOrderraw"] = RoomNameOrder;
            ViewData["PriceOrderraw"] = PriceOrder;
            ViewData["FloorOrderraw"] = FloorOrder;
            ViewData["AreaIdraw"] = AreaId;
            CustomSelection();
            if (_context.Rooms != null)
            {
               var Roomquery = _context.Rooms
                .Include(r => r.Area)
                .Include(r => r.Floor)
                .Include(r => r.NumOfPerson)
                .Include(r => r.Price).ToList().
                Where(x =>
                  (string.IsNullOrEmpty(RoomId) || x.RoomId == int.Parse(RoomId))
                && (string.IsNullOrEmpty(RoomName) || x.RoomName.Trim().Contains(RoomName.Trim()))
                && (string.IsNullOrEmpty(NumOfPersonId) || x.NumOfPersonId == int.Parse(NumOfPersonId))
                && (string.IsNullOrEmpty(FloorId) || x.FloorId == int.Parse(FloorId))
                && (string.IsNullOrEmpty(AreaId) || x.AreaId == int.Parse(AreaId))
                && (string.IsNullOrEmpty(PriceId) || x.PriceId == int.Parse(PriceId)));

                if (!string.IsNullOrEmpty(RoomNameOrder) && int.Parse(RoomNameOrder) == 2) 
                {
                    Roomquery = Roomquery.OrderByDescending(x => int.Parse(x.RoomName));
                }
                else if(!string.IsNullOrEmpty(RoomNameOrder) && int.Parse(RoomNameOrder) == 1)
                {
                    Roomquery = Roomquery.OrderBy(x => int.Parse(x.RoomName));
                }
                else
                {

                }

                if (!string.IsNullOrEmpty(PriceOrder) && int.Parse(PriceOrder) == 2)
                {
                    Roomquery = Roomquery.OrderByDescending(x => x.Price.Price);
                }
                else if (!string.IsNullOrEmpty(PriceOrder) && int.Parse(PriceOrder) == 1)
                {
                    Roomquery = Roomquery.OrderBy(x => x.Price.Price);
                }
                else
                {

                }

                if (!string.IsNullOrEmpty(FloorOrder) && int.Parse(FloorOrder) == 2) 
                {
                    Roomquery = Roomquery.OrderByDescending(x => x.Floor.Floor1);
                }
                else if (!string.IsNullOrEmpty(FloorOrder) && int.Parse(FloorOrder) == 1)
                {
                    Roomquery = Roomquery.OrderBy(x => x.Floor.Floor1);
                }
                else
                {

                }

                Room = Roomquery.ToList();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            SelectedRooms = new List<int>();
            var selectedRoomIds = Request.Form["selectedRooms"];
            string listRejectDeleteAction = "Phòng ";
            bool isHasCust = false;
            List<Models.Room> List = new List<Models.Room>();
            foreach (var item in selectedRoomIds)
            {
                if (int.TryParse(item, out int parsedRoomId))
                {
                    Models.Room r = _context.Rooms.Include(x => x.Customers).FirstOrDefault(r => r.RoomId == parsedRoomId);
                    if(r.NumOfLiving > 0)
                    {
                        listRejectDeleteAction += r.RoomName + "  ";
                        isHasCust= true;
                    }
                    else
                    {
                        List.Add(r);
                    }
                    _context.SaveChanges();
                }
            }
            listRejectDeleteAction += " đang có người ở không thể xóa";
            if (isHasCust == true)
            {
                TempData["ErrorMessage"] = listRejectDeleteAction;

            }
            _context.Rooms.RemoveRange(List);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }

        public void CustomSelection()
        {
            var areaOptions = _context.Areas.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Area1.ToString() }).ToList();
            areaOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn Diện Tích --" });
            var floorOptions = _context.Floors.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Floor1.ToString() }).ToList();
            floorOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn Tầng --" });
            var numOptions = _context.NumOfPeople.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.NumOfPerson1.ToString() }).ToList();
            numOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn Số Người Ở Tối Đa --" });
            var priceOptions = _context.RoomPrices.Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Price.ToString() }).ToList();
            priceOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn Giá Phòng --" });

            ViewData["AreaId"] = new SelectList(areaOptions, "Value", "Text");
            ViewData["FloorId"] = new SelectList(floorOptions, "Value", "Text");
            ViewData["NumOfPersonId"] = new SelectList(numOptions, "Value", "Text");
            ViewData["PriceId"] = new SelectList(priceOptions, "Value", "Text");
        }

        public async Task<IActionResult> OnSelectChange()
        {
            return RedirectToPage("./Index");
        }
    }
}
