using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OAuthLab.DAL.Entities;

namespace OAuthLab.GAuth.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly StoreSampleContext _context;

        public IndexModel(StoreSampleContext context)
        {
            _context = context;
        }

        public IList<Order> Order { get;set; }

        public async Task OnGetAsync()
        {
            Order = await _context.Order
                .Include(o => o.Customer).ToListAsync();
        }
    }
}
