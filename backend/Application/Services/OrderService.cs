using Application.DTOs.Create;
using Application.DTOs.Responses;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto)
        {
            _logger.LogInformation("Iniciando criação de pedido para ClienteId={ClientId} com {ItemsCount} itens.",
                dto.ClientId, dto.Items.Count);

            var order = new Order
            {
                Id = Guid.NewGuid(),
                ClientId = dto.ClientId,
                OrderDate = DateTime.UtcNow,
                Items = dto.Items.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                }).ToList()
            };

            await _orderRepository.AddAsync(order);
            _logger.LogInformation("Pedido criado e persistido. OrderId={OrderId}, ClienteId={ClientId}",
                order.Id, order.ClientId);

            // Recarrega o pedido com relacionamentos e retorna DTO
            var created = await _orderRepository.GetByIdAsync(order.Id);
            if (created == null)
            {
                _logger.LogError("Falha ao recarregar pedido recém-criado. OrderId={OrderId}", order.Id);
                throw new InvalidOperationException("Não foi possível carregar o pedido recém-criado.");
            }

            var response = MapToResponseDto(created);
            _logger.LogInformation("Pedido criado com sucesso. OrderId={OrderId}, Total={Total}",
                response.Id, response.TotalValue);

            return response;
        }

        public async Task<OrderResponseDto?> GetByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                _logger.LogWarning("Pedido não encontrado. OrderId={OrderId}", id);
                return null;
            }

            return MapToResponseDto(order);
        }

        public async Task<IEnumerable<OrderResponseDto>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var list = orders.Select(MapToResponseDto).ToList();
            _logger.LogInformation("Listando {Count} pedidos.", list.Count);
            return list;
        }

        private static OrderResponseDto MapToResponseDto(Order order)
        {
            return new OrderResponseDto
            {
                Id = order.Id,
                ClientName = order.Client.Name,
                OrderDate = order.OrderDate,
                Items = order.Items.Select(i => new OrderItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList(),
                TotalValue = order.TotalValue
            };
        }
    }

}