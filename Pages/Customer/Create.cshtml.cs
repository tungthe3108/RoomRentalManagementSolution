using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.Customer
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
            var roomOptions = _context.Rooms.Select(a => new SelectListItem { Value = a.RoomId.ToString(), Text = a.RoomName.ToString() }).ToList();
            roomOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn phòng --" });
            ViewData["RoomId"] = new SelectList(roomOptions, "Value", "Text");
            return Page();
        }

        [BindProperty]
        public Models.Customer Customer { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var roomOptions = _context.Rooms.Select(a => new SelectListItem { Value = a.RoomId.ToString(), Text = a.RoomName.ToString() }).ToList();
            roomOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn phòng --" });
            ViewData["RoomId"] = new SelectList(roomOptions, "Value", "Text");

            var room = _context.Rooms.Include(r => r.NumOfPerson).FirstOrDefault(x => x.RoomId == Customer.RoomId);
            if (room.NumOfPerson.NumOfPerson1 == room.NumOfLiving)
            {
                TempData["ErrorMessage"] = "Phòng " + room.RoomName + " đã đầy vui lòng chọn phòng khác!";
                return Page();
            }
            room.NumOfLiving += 1;
            _context.Rooms.Update(room);
            _context.Customers.Add(Customer);
            
            await _context.SaveChangesAsync();


            TempData["SuccessMessage"] = "Thêm khách hàng " + Customer.Name + " thành công!";
            return Page();
        }

    }
}
