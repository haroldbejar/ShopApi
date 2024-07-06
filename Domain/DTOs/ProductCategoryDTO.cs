using Domain.Entities;

namespace Domain.DTOs
{
    public class ProductCategoryDTO
    {
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
