namespace Application.DTOs.Create
{
    public class CreateOrderDto
    {
        public Guid ClientId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();
    }
}
