using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopCart.Infrastructure;

namespace ShopCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopCartContext context;

        public CartController(ShopCartContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}