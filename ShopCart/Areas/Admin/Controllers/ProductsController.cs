 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopCart.Infrastructure;
using ShopCart.Models;

namespace ShopCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShopCartContext context;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductsController(ShopCartContext context, IWebHostEnvironment env)
        {
            this.context = context;
            webHostEnvironment = env;
        }


        ///admin/products because its called index
        public async Task<IActionResult> Index(int p = 1)
        {
            //ttps://docs.microsoft.com/en-us/aspnet/mvc/overview/getting-started/getting-started-with-ef-using-mvc/reading-related-data-with-the-entity-framework-in-an-asp-net-mvc-application
            //TODO INCLUDE() HERE MEANS Eager loading this one query(product and category) instead of 2 separate 
            //TODO ask what this linq query actaally do :(((

            int pageSize = 6;
            var products = context.Products.OrderByDescending(x => x.Id)
                                            .Include(x => x.Category)
                                            .Skip((p - 1) * pageSize)
                                            .Take(pageSize);

            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)context.Products.Count() / pageSize);
            //View(await context.Products.OrderByDescending(x => x.Id).Include(x => x.Category).ToListAsync()); this gets all of the pages
            return View(await products.ToListAsync());
        }
        //admin/products/create
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }

        //POST  admin/products/create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                //IT checks if slug exists NOT to have products with the same name!
                var slug = await context.Products.FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(product);
                }

                //TODO what is path.combine and file streams
                string imageName = "noimage.png";
                if(product.ImageUpload != null)
                { 
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName; // this gives unique id so no same image twice uploaded 
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fileStream);
                    fileStream.Close();
                }
                product.Image = imageName;




                context.Add(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been added.";

                return RedirectToAction("Index");
            }

            return View(product);
        }
        //admin/products/details/{id}

        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        //admin/products/edit/{id}
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);


            return View(product);
        }

        //POST  admin/products/edit/{id}        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                product.Slug = product.Name.ToLower().Replace(" ", "-");

                //IT checks if slug exists NOT to have products with the same name!
                var slug = await context.Products.Where(x => x.Id != id).FirstOrDefaultAsync(x => x.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "The product already exists.");
                    return View(product);
                }

                
                
                if (product.ImageUpload != null)
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");

                    if(!string.Equals(product.Image, "noimage.png")) //if it is not noimage then we have to delete the image
                    {
                        string oldImagePath = Path.Combine(uploadsDir, product.Image);
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }

                    }

                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName; // this gives unique id so no same image twice uploaded 
                    string filePath = Path.Combine(uploadsDir, imageName);
                    FileStream fileStream = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpload.CopyToAsync(fileStream);
                    fileStream.Close();
                    product.Image = imageName;
                }
                
                context.Update(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been edited.";

                return RedirectToAction("Index");
            }

            return View(product);
        }

        //GET /admin/products/delete/{id}
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FindAsync(id);


            if (product == null)
            {
                TempData["Error"] = "The product does not exist.";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png")) //if it is not noimage then we have to delete the image
                {
                    string uploadsDir = Path.Combine(webHostEnvironment.WebRootPath, "media/products");
                    string oldImagePath = Path.Combine(uploadsDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();

                TempData["Success"] = "The product has been removed.";
            }

            return RedirectToAction("Index");
        }

    }
}