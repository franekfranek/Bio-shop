using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Infrastructure
{
    public class CategoriesViewComponent : ViewComponent
    {
        private readonly ShopCartContext context;
        public CategoriesViewComponent(ShopCartContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await GetCategoriesAsync();
            return View(categories);
        }

        private Task<List<Category>> GetCategoriesAsync()
        {
            //it return list so model in html file is specified as IEnumerable
            return context.Categories.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
