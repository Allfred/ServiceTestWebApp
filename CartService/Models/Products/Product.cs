using CartService.Models.Base;

namespace CartService.Models.Products
{
    public class Product : IEntities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public bool ForBonusPoints { get; set; }
    
    }
}
