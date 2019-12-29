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
    public class PagesController : Controller
    {
        private readonly ShopCartContext context;

        public PagesController(ShopCartContext context)
        {
            this.context = context;
        }
        //GET //admin/pages
        public async Task<IActionResult> Index()
        {
            IQueryable<Page> pages = from p in context.Pages orderby p.Sorting select p;

            List<Page> pagesList = await pages.ToListAsync();

            //ViewBag.Fruit = "Orange";   if u want to pass extra data to page not included in model(Page here) use ViewBag 

            return View(pagesList);
        }
        //GET //admin/pages/details/{id}
        public async Task<IActionResult> Details(int id)
        {
            Page page = await context.Pages.FirstOrDefaultAsync(x => x.Id == id);
            if(page == null)
            {
                return NotFound();
            }

            return View(page);
        }
        //GET!!!! //admin/pages/create
        public IActionResult Create() => View();

        //POST //admin/pages/create
        [HttpPost] //If you not specify request attribute it is get by default
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Page page) // Page is passed because Create.cshtml uses @model Page (on top)
        {
            if (ModelState.IsValid)
            {
                page.Slug = page.Title.ToLower().Replace(" ", "-");
                page.Sorting = 100;

                var slug = await context.Pages.FirstOrDefaultAsync(x => x.Slug == page.Slug);
                if(slug != null)
                {
                    ModelState.AddModelError("", "The title already exists.");
                    return View(page);
                }
                context.Add(page);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been added.";

                return RedirectToAction("Index");
            }

            return View(page);
        }
    }
}