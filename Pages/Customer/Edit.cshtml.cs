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
    public class EditModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public EditModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Customer Customer { get; set; } = default!;
        public int roomIdraw;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            string userName = HttpContext.Session.GetString("username");
            if (userName == null)
            {
                return RedirectToPage("/Login/Index");
            }
            if (id == null || _context.Customers == null)
            {
                return NotFound();
            }

            var customer =  await _context.Customers.FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customer == null)
            {
                return NotFound();
            }
            var roomOptions = _context.Rooms.Select(a => new SelectListItem { Value = a.RoomId.ToString(), Text = a.RoomName.ToString() }).ToList();
            roomOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn phòng --" });
            ViewData["RoomId"] = new SelectList(roomOptions, "Value", "Text");
            roomIdraw = (int)customer.RoomId;
            Customer = customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id, int? roomId)
        {
            var roomOptions = _context.Rooms.Select(a => new SelectListItem { Value = a.RoomId.ToString(), Text = a.RoomName.ToString() }).ToList();
            roomOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn phòng --" });
            ViewData["RoomId"] = new SelectList(roomOptions, "Value", "Text");

            

            var room2 = _context.Rooms.Include(r => r.NumOfPerson).FirstOrDefault(x => x.RoomId == Customer.RoomId);
            var room1 = _context.Rooms.FirstOrDefault(x => x.RoomId == roomId);
            if (room2.NumOfPerson.NumOfPerson1 == room2.NumOfLiving && room1.RoomId != room2.RoomId)
            {
                TempData["ErrorMessage"] = "Phòng " + room2.RoomName + " đã đầy vui lòng chọn phòng khác!" ;
                return Page();
            }

            room1.NumOfLiving -= 1;
            room2.NumOfLiving += 1;
            _context.Rooms.Update(room1);
            _context.Rooms.Update(room2);
            _context.Customers.Update(Customer);
            try
            {

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm dữ liệu thành công!";

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(Customer.CustomerId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Page();
        }

        private bool CustomerExists(int id)
        {
          return (_context.Customers?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
    }
}
