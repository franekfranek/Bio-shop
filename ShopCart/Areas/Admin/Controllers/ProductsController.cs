 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopCart.Infrastructure;

namespace ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopCartContext context;
        public ProductsController(ShopCartContext context)
        {
            this.context = context;
        }


        ///admin/products because its called index
        public async Task<IActionResult> Index()
        {
            //ttps://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/reading-related-data-with-the-entity-framework-in-an-asp-net-mvc-application
            //TODO INCLUDE() HERE MEANS Eager loading this one query(product and category) instead of 2 separate 

            return View(await context.Products.OrderByDescending(x => x.Id).Include(x => x.Category).ToListAsync());
        }
        //admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }

    }
}