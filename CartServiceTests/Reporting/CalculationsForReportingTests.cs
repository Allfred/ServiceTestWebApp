using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using CartService.Models.Carts;
using CartService.Models.Products;

namespace CartService.Reporting.Tests
{
    [TestFixture()]
    public class CalculationsForReportingTests
    {
        private static List<Product> products = new List<Product>()
        {
            new Product(){Id=1,Name="1",Cost=3,ForBonusPoints=true},
            new Product(){Id=2,Name="2",Cost=7,ForBonusPoints=false}
        };

        private static List<Cart> carts = new List<Cart>() 
        {
        
            new Cart(){ Id=1,Name="1", CreatedDateTime = DateTime.Today.AddDays(-10), Products = new List<Product>()},
            new Cart(){ Id=1,Name="2", CreatedDateTime = DateTime.Today.AddDays(-15), Products = products},
            new Cart(){ Id=1,Name="3", CreatedDateTime = DateTime.Today.AddDays(-25), Products = products.Where(x=>x.ForBonusPoints)},
            new Cart(){ Id=1,Name="4", CreatedDateTime = DateTime.Today.AddDays(-35), Products = products.Where(x=>!x.ForBonusPoints)},
        
        };

        [Test()]
        public void CalcCartAverageCostTest()
        {
            var rightResult = 5;
            var result = CalculationsForReporting.CalcCartAverageCost(carts);
            Assert.IsTrue(rightResult == result);
        }

        [Test()]
        public void CountOfCartsWithBonusTest()
        {
            var rightResult = 2;

            var result = CalculationsForReporting.CountOfCartsWithBonus(carts);
            Assert.IsTrue(rightResult == result);
        }

        [Test()]
        public void CountOfСartExpiresInDaysTest()
        {
            var days = new List<int> { 10, 20, 30 };
            var rightResults = new List<int> { 1, 2, 3 };
             
            for(int i=0; i < days.Count; i++)
            {
                var result = CalculationsForReporting.CountOfСartExpiresInDays(carts,days[i]);
                
                Assert.IsTrue(rightResults[i] == result);
            }
        }
    }
}
