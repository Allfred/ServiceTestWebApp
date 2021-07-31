using System;
using System.Collections.Generic;
using CartService.Models.Base;
using CartService.Models.Products;

namespace CartService.Models.Carts
{
    public class Cart : IEntities
    {
        public Cart()
        {
            Products = new List<Product>();
        }
        public int Id { get; set; }

        public string Name { get; set; }
        
        public DateTime CreatedDateTime { get; set; }

        public IEnumerable<Product> Products { get; set; }

    }
}
