using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.Room
{
    public class DetailsModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public DetailsModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }
        public IList<Models.Customer> Customer { get; set; } = default!;
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
            Customer = _context.Customers.Where(x => x.RoomId== id).ToList();   

            var room = _context.Rooms.Include(r => r.Area)
                .Include(r => r.Floor)
                .Include(r => r.NumOfPerson)
                .Include(r => r.Price).FirstOrDefault(m => m.RoomId == id);
            if (room == null)
            {
                return NotFound();
            }
            else 
            {
                Room = room;
            }
            return Page();
        }
    }
}
