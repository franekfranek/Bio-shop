using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Infrastructure
{
    //viewComponent suffix is the same as attribute suffix (file extension e.x)
    // dependiecies in viewcomponent are put in constructor
    // view components have nothoing to do with either requests or model biding
    // default return view(no parameters) look in SHARED/COMPONENTS/{ViewComponent}/Default.cshtml
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly ShopCartContext context;
        public MainMenuViewComponent(ShopCartContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPagesAsync();
            return View(pages);
        }

        private Task<List<Page>> GetPagesAsync()
        {
            //it return list so model in html file is specified as IEnumerable
            return context.Pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
