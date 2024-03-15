using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.RoomAttribute
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
            return Page();
        }

        [BindProperty]
        public Area Area { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Areas == null || Area == null)
            {
                return Page();
            }

            _context.Areas.Add(Area);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm diện tích phòng: "  + Area.Area1 + " thành công!";

            return Page();
        }
    }
}
