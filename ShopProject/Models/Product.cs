using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        [Range(0,20000)]
        public float Price { get; set; }
        [Required]
        public string ImgURL { get; set; }

        public ICollection<ProductInCart> productInCarts { get; set; }
    }
}
