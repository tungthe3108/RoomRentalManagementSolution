using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.RoomAttribute.price
{
    public class DeleteModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public DeleteModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }

        [BindProperty]
      public RoomPrice RoomPrice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.RoomPrices == null)
            {
                return NotFound();
            }

            var roomprice = await _context.RoomPrices.FirstOrDefaultAsync(m => m.Id == id);

            if (roomprice == null)
            {
                return NotFound();
            }
            else 
            {
                RoomPrice = roomprice;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.RoomPrices == null)
            {
                return NotFound();
            }
            var roomprice = await _context.RoomPrices.FindAsync(id);

            if (roomprice != null)
            {
                RoomPrice = roomprice;
                _context.RoomPrices.Remove(RoomPrice);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
