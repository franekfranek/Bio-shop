using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopCart.Infrastructure
{
    public class ShopCartContest : DbContext

    {
        public ShopCartContest(DbContextOptions<ShopCartContest> options) :base(options)
        {

        }
    }
}
