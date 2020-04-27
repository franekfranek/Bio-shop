using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ShopCart.Infrastructure;
using ShopCart.Models;

namespace ShopCart.Controllers
{
    [Area("General")]
    //[Route("General/[controller]/[action]")]
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
            // WHAT IS "Cart" here

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartViewModel cartVM = new CartViewModel
            {
                CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)

            };

            return View(cartVM);
        }

        // GET /cart/add/{id}
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if(cartItem == null)
            {
                cart.Add(new CartItem(product));
            }
            else
            {
                cartItem.Quantity += 1;
            }

            //HttpContext.Session.Set   ESTABLISCH SESSION FOR OUR CART READY METHOD CAN BE USED --> SET BUT IT IS ONLY FOR INT32 AND
            // STRING SO CUSTOM SETJESON IS ADDED TO USE LIST FOR EXAMPLE (FOR COMPLEX COLLECTION U HAVE TO SERIALIZE THEM)

            HttpContext.Session.SetJson("Cart", cart);


            if(HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");
           
        }

        //get general/cart/decrease/{id}

        public IActionResult Decrease(int id)
        {
            //cart session exists in this context so we dont need new List (and double question marks)

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;   //TODO check is there any diffrence 
             }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }

            

            if(cart.Count() == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        //get /cart/remove/{id}
        public IActionResult Remove(int id)
        {
            //cart session exists in this context so we dont need new List (and double question marks)

            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            cart.RemoveAll(x => x.ProductId == id);


            if (cart.Count() == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return RedirectToAction("Index"); 
        }

        //get /cart/clear/
        public IActionResult Clear()
        {
            //cart session exists in this context so we dont need new List (and double question marks)
            
            HttpContext.Session.Remove("Cart");
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return Ok();

            
            //return Redirect("/");
            //return RedirectToAction("Index", "Products");
        }
    }
}

