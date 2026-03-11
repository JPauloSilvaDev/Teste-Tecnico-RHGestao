namespace Application.DTOs.Responses
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public string ClientName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();

        public decimal TotalValue { get; set; }

    }
}
