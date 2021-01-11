using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string Passowrd { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }

        public ICollection<ShoppingCart> shoppingCart { get; set; }
    }
}
