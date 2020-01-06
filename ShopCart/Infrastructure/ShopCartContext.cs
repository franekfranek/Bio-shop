using Microsoft.EntityFrameworkCore;
using ShopCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Infrastructure
{
    //  ON DELETE CASCADE IN SQL MEANS IF YOU DELETE COLUMN ALL ASSOCIATED (BY FOREIGN KEY) COLUMNS DELETED E.X IF U REMOVE CATEGORY ALL PRODUCTS OF THIS C. ARE REMOVED
    // ctrl shift space to see list of overloads(what arguments can be passed to a method)
    public class ShopCartContext : DbContext

    {
        public ShopCartContext(DbContextOptions<ShopCartContext> options) :base(options)
        {

        }
        public DbSet<Page> Pages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
