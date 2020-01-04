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

        //admin/categories/edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        //post /admin/categories/edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
                //TODO why id is passed here
        {
            if (ModelState.IsValid)
            {
                category.Slug = category.Name.ToLower().Replace(" ", "-");


                var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == category.Slug);
                if (slug != null) //it exists     it checks if category exists already
                {
                    ModelState.AddModelError("", "The category already exists.");
                    return View(category);
                }
                context.Update(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The page has been edited.";

                return RedirectToAction("Edit", new { id = id }); //TODO what is this and how it works check
            }

            return View(category);
        }

        //GET /admin/categories/delete/{id}   IT CAN BE POST AS WELL
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);

            if (category == null)
            {
                TempData["Error"] = "The category does not exist.";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                TempData["Success"] = "The category has been removed.";
            }

            return RedirectToAction("Index");
        }

        //post /admin/categories/reorder
        [HttpPost]
        public async Task<IActionResult> Reorder(int[] id) //it changes the sorting values depends on d&d  
        {
            int count = 1;

            foreach (var categoryId in id)
            {
                Category category = await context.Categories.FindAsync(categoryId); //is the same as ...Where(x => x.Id = pageId)
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok(); // it returns status 200 ok response :D:D:D
        }

    }
}