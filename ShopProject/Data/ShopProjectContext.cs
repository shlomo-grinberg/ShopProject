using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace ShopProject.Data
{
    public class ShopProjectContext : DbContext
    {
        public ShopProjectContext (DbContextOptions<ShopProjectContext> options)
            : base(options)
        {
        }

        public DbSet<Shop.Models.Product> Product { get; set; }

        public DbSet<Shop.Models.ProductInCart> ProductInCart { get; set; }

        public DbSet<Shop.Models.ShoppingCart> ShoppingCart { get; set; }

        public DbSet<Shop.Models.User> User { get; set; }
    }
}
