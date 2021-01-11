using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class ProductInCart
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product product { get; set; }

        [Required]
        public int Amount { get; set; }
        [Required]
        public float FinalPrice { get; set; }

        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }

    }
}
