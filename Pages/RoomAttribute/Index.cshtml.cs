using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.RoomAttribute
{
    public class IndexModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public IndexModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }
        public IList<RoomPrice> RoomPrice { get; set; } = default!;
        public IList<Area> Area { get;set; } = default!;
        public async Task OnGetAsync(string AreaInput, string PriceInput)
        {
            if (_context.Areas != null)
            {
                Area = await _context.Areas.ToListAsync();
            }
            if (_context.RoomPrices != null)
            {
                RoomPrice = await _context.RoomPrices.ToListAsync();
            }
            //if(!string.IsNullOrEmpty(AreaInput.Trim())  || !string.IsNullOrEmpty(PriceInput.Trim()))
            //{
            //    try
            //    {
            //        int area = int.Parse(AreaInput);
            //        int price = int.Parse(PriceInput);
            //        if(AreaInput != null)
            //        {
            //            Area a = new Area();
            //            a.Area1 = area;
            //            _context.Areas.Add(a);
            //            _context.SaveChanges();
            //        }
            //        if(PriceInput != null)
            //        {
            //            RoomPrice p = new RoomPrice();
            //            p.Price = price;
            //            _context.RoomPrices.Add(p);
            //            _context.SaveChanges();
            //        }
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var selectedPriceIds = Request.Form["selectedPricesId"];
            var selectedAreasIds = Request.Form["selectedAreasId"];
            List<Models.RoomPrice> List = new List<Models.RoomPrice>();
            string listPriceRejectDeleteAction = "Giá ";
            bool isHasRoomPrice = false;
            foreach (var item in selectedPriceIds)
            {
                if (int.TryParse(item, out int priceId))
                {
                    Models.RoomPrice r = _context.RoomPrices.FirstOrDefault(r => r.Id == priceId);
                    Models.Room room  = _context.Rooms.Include(x => x.Price).FirstOrDefault(x => x.PriceId== priceId);
                    if(room != null)
                    {
                        listPriceRejectDeleteAction += r.Price + " ";
                        isHasRoomPrice = true;
                    }
                    else
                    {
                        List.Add(r);
                    }
                }
            }
            listPriceRejectDeleteAction += " đang sử dụng không thể xóa";
            if (isHasRoomPrice == true)
            {
                TempData["ErrorMessage"] = listPriceRejectDeleteAction;

            }

            List<Models.Area> ListA = new List<Models.Area>();
            string listAreaRejectDeleteAction = "Diện tích ";
            bool isHasRoomArea = false;
            foreach (var item in selectedAreasIds)
            {
                if (int.TryParse(item, out int AreaId))
                {
                    Models.Area r = _context.Areas.FirstOrDefault(r => r.Id == AreaId);
                    Models.Room room = _context.Rooms.Include(x => x.Price).FirstOrDefault(x => x.AreaId == AreaId);
                    if (room != null)
                    {
                        listAreaRejectDeleteAction += r.Area1 + " ";
                        isHasRoomArea = true;
                    }
                    else
                    {
                        ListA.Add(r);
                    }
                }
            }
            listAreaRejectDeleteAction += " đang sử dụng không thể xóa";
            if (isHasRoomArea == true)
            {
                TempData["ErrorMessage2"] = listAreaRejectDeleteAction;

            }
            _context.RoomPrices.RemoveRange(List);
            _context.Areas.RemoveRange(ListA);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }

    }
}
