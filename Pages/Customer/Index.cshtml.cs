using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;
namespace RoomRentalManagementSolution.Pages.Customer
{
    public class IndexModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public IndexModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }

        public IList<Models.Customer> Customer { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync(string custName, string gender, string dob, string phone, string email, 
            string address, string roomId)
        {
            string userName = HttpContext.Session.GetString("username");
            if (userName == null)
            {
                return RedirectToPage("/Login/Index");
            }
            ViewData["custNameraw"] = custName;
            ViewData["genderraw"] = gender;
            ViewData["dobraw"] = dob;
            ViewData["phoneraw"] = phone;
            ViewData["emailraw"] = email;
            ViewData["addressraw"] = address;
            ViewData["roomIdraw"] = roomId;
            if (_context.Customers != null)
            {
                Customer = _context.Customers
                .Include(c => c.Room).
                Where(x =>
                  (string.IsNullOrEmpty(custName) || x.Name.Trim().Contains(custName.Trim()))
                && (string.IsNullOrEmpty(gender) || x.Sex.Trim().Contains(gender.Trim()))
                && (string.IsNullOrEmpty(dob) || x.Dob.Date == DateTime.Parse(dob))
                && (string.IsNullOrEmpty(phone) || x.Phone.ToString().Trim().Contains(phone.Trim()))
                && (string.IsNullOrEmpty(email) || x.Email.Trim().Contains(email.Trim()))
                && (string.IsNullOrEmpty(address) || x.Address.Trim().Contains(address.Trim()))
                && (string.IsNullOrEmpty(roomId) || x.RoomId == int.Parse(roomId))
                ).ToList();
            }
            var roomOptions = _context.Rooms.Select(a => new SelectListItem { Value = a.RoomId.ToString(), Text = a.RoomName.ToString() }).ToList();
            roomOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn phòng --" });
            ViewData["RoomId"] = new SelectList(roomOptions, "Value", "Text");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roomOptions = _context.Rooms.Select(a => new SelectListItem { Value = a.RoomId.ToString(), Text = a.RoomName.ToString() }).ToList();
            roomOptions.Insert(0, new SelectListItem { Value = null, Text = "-- Chọn phòng --" });
            ViewData["RoomId"] = new SelectList(roomOptions, "Value", "Text");

            var selectedRoomIds = Request.Form["selectedCustomer"];
            List<Models.Customer> List = new List<Models.Customer>();
            foreach (var item in selectedRoomIds)
            {
                if (int.TryParse(item, out int parsedCustId))
                {
                    Models.Customer c = _context.Customers.Include(x => x.Rooms).FirstOrDefault(r => r.CustomerId == parsedCustId);
                    Models.Room r = _context.Rooms.FirstOrDefault(x => x.RoomId == c.RoomId);
                    r.NumOfLiving -= 1;
                    _context.Rooms.Update(r);
                    _context.SaveChanges();
                    List.Add(c);
                }
            }
            _context.Customers.RemoveRange(List);
            _context.SaveChanges();
            return RedirectToPage("./Index");
        }
    }
}
