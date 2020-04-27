using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCart.Infrastructure;
using ShopCart.Models;

namespace ShopCart.Controllers
{
    [Area("General")]
    [Route("General/[controller]/[action]")]
    public class ProductsController : Controller
    {
        private readonly ShopCartContext context;
        //private readonly IWebHostEnvironment webHostEnvironment;
        public ProductsController(ShopCartContext context)
        {
            this.context = context;
            
        }
        //GET /products
        public async Task<IActionResult> Index(int p = 1)
        {
            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);
            return View(await products.ToListAsync());
        }
        // /products/category
        public async Task<IActionResult> ProductsByCategory(string categorySlug, int p = 1)
        {

            Category category = await context.Categories.Where(x => x.Slug == categorySlug).FirstOrDefaultAsync();
            if(category == null) return await Index();



            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Where(x => x.Category.Id == category.Id )
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products
                                            .Where(x => x.Category.Id == category.Id)
                                            .Count() / pageSize);
            ViewBag.CategoryName = category.Name;
            @ViewBag.CategorySlug = category.Slug;

            return View(await products.ToListAsync());
        }
        
    }
}