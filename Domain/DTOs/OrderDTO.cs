using Domain.Entities;

namespace Domain.DTOs
{
    public class OrderDTO
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string Description { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
