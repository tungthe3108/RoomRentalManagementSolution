using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoomRentalManagementSolution.Models;

namespace RoomRentalManagementSolution.Pages.Login
{
    public class IndexModel : PageModel
    {
        private readonly RoomRentalManagementSolution.Models.RoomRetalManagementContext _context;

        public IndexModel(RoomRentalManagementSolution.Models.RoomRetalManagementContext context)
        {
            _context = context;
        }

        public IList<Account> Account { get;set; } = default!;

        public async Task OnGetAsync()
        {
        }

        public async Task<IActionResult> OnPostAsync(string username, string password)
        {
            var account = _context.Accounts.FirstOrDefault(x => x.Username == username 
            && x.Password == password);
            if (account == null)
            {
                TempData["ErrorMessage"] = "Tài khoản hoặc mật khẩu sai!";
                return Page();
            }
            else {
                if(account.Role != "admin")
                {
                    TempData["ErrorMessage"] = "Tài khoản không có quyền truy cập!";
                    return Page();
                }
                else if(account.Role == "admin")
                {
                    HttpContext.Session.SetString("username", account.Name);
                    return RedirectToPage("/Room/Index");
                }
                return Page();
            }
         
        }
    }
}
