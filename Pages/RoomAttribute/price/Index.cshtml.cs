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
    public class IndexModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public IndexModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }

        public IList<RoomPrice> RoomPrice { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.RoomPrices != null)
            {
                RoomPrice = await _context.RoomPrices.ToListAsync();
            }
        }
    }
}
