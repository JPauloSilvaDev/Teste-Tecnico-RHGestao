namespace Application.DTOs.Update
{
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int QuantityInStock { get; set; }
    }
}

