using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }

        public DateTime DateCreate { get; set; }

        public int UserId { get; set; }
        public User user { get; set; }

        public ICollection<ProductInCart> products { get; set; }
    }
}
