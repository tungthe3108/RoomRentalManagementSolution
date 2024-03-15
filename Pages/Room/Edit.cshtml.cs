using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.Room
{
    public class EditModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public EditModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RoomRentalManagementSolution.Models.Room Room { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userName = HttpContext.Session.GetString("username");
            if (userName == null)
            {
                return RedirectToPage("/Login/Index");
            }
            if (id == null || _context.Rooms == null)
            {
                return NotFound();
            }

            var room =  await _context.Rooms.FirstOrDefaultAsync(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            Room = room;
            CustomSelection();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            CustomSelection();

            _context.Attach(Room).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(Room.RoomId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool RoomExists(int id)
        {
          return (_context.Rooms?.Any(e => e.RoomId == id)).GetValueOrDefault();
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
