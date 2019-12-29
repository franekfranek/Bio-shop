using Microsoft.EntityFrameworkCore;
using ShopCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Infrastructure
{
    public class ShopCartContext : DbContext

    {
        public ShopCartContext(DbContextOptions<ShopCartContext> options) :base(options)
        {

        }
        public DbSet<Page> Pages { get; set; }
    }
}
