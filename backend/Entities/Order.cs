namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public Guid ClientId { get; set; }
         
        public Customer Client { get; set; } = null!;

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

        public ICollection<Product> Products { get; set; } = new List<Product>();

        public decimal TotalValue => Items.Sum(i => i.Quantity * i.UnitPrice);


    }
}
