using Application.DTOs.Create;
using Application.DTOs.Responses;

namespace Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto);
        Task<OrderResponseDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<OrderResponseDto>> GetAllAsync();
    }
}
