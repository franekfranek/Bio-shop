using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopCart.Infrastructure;
using ShopCart.Models;

namespace ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ShopCartContext context;
        public CategoriesController(ShopCartContext context)
        {
            this.context = context;
        }
        //   /admin/categories    because its called index
        public async Task<IActionResult> Index()
        {

            return View(await context.Categories.OrderBy(x => x.Sorting).ToListAsync());
        }

        //GET //admin/categories/create
        public IActionResult Create() => View();

        //POST  admin/categories/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");
                category.Sorting = 100;

                //IT checks if slug exists NOT to have categories with the same name!
                var slug = await context.Categories.FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The category already exists.");
                    return View(category);
                }
                context.Add(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The category has been added.";

                return RedirectToAction("Index");
            }

            return View(category);
        }
    }
}