using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopCart.Infrastructure;
using ShopCart.Models;

namespace ShopCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopCartContext context;

        public CartController(ShopCartContext context)
        {
            this.context = context;
        }
        // GET /cart
        public IActionResult Index()
        {   
            //TODO what is double question mark
            //FIRST BELOW IS FROM SESSION AND IF IT IS NULL CREATE A NEW LIST OF CART ITEMS
            //TODO DTO is DATA TRANSFER OBJECT CLASSES FROM MODEL HERE DTO'S

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)

            };

            return View(cartVM);
        }
    }
}