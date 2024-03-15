using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.Room
{
    public class CreateModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public CreateModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }
        public IActionResult OnGet()
        {
            string userName = HttpContext.Session.GetString("username");
            if (userName == null)
            {
                return RedirectToPage("/Login/Index");
            }
            CustomSelection();
            return Page();
        }

        [BindProperty]
        public RoomRentalManagementSolution.Models.Room Room { get; set; } = default!;


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            CustomSelection();
            if (_context.Rooms.FirstOrDefault(x => x.RoomName.Trim() == Room.RoomName.Trim()) != null)
            {
                ModelState.AddModelError("Room.RoomName", "Tên phòng đã tồn tại");
                return Page();
            };
            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm phòng " + Room.RoomName + " thành công!";
            return Page();
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
    }
}
