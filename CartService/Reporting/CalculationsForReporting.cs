using System;
using System.Collections.Generic;
using System.Linq;
using CartService.Models.Carts;

namespace CartService.Reporting
{
    public static class CalculationsForReporting
    {
        public static decimal CalcCartAverageCost(List<Cart> carts)
        {
            if (!carts.Any()) return 0;
            
            return carts.Where(cart => cart.Products.Any())
                .Average(cart => cart.Products.Average(p => p.Cost));
        }

        public static int CountOfCartsWithBonus(List<Cart> carts)
        {
            return carts.Count(cart => cart.Products.Any(product => product.ForBonusPoints));
        }

        public static int CountOfСartExpiresInDays(List<Cart> carts, int day)
        {
            var today = DateTime.Now;

            var count = carts.Count(cart => (today - cart.CreatedDateTime).Days <= day);
            return count;
        }
    }
}