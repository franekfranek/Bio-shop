using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCart.Infrastructure;
using ShopCart.Models;

namespace ShopCart.Controllers
{
    //ctrl shift enter new line
    public class PagesController : Controller
    {
        private readonly ShopCartContext context;
        public PagesController(ShopCartContext context)
        {
            this.context = context;
        }

        // get / or /slug
        public async Task<IActionResult> Page(string slug)
        {
            if(slug == null)
            {
                return View(await context.Pages.Where(x => x.Slug == "home").FirstOrDefaultAsync());
            }

            Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();

            if(page == null)
            {
                return NotFound();
            }
            return View(page);
        }
    }
}