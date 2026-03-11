using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();
            _logger.LogInformation("Listando {Count} produtos.", products.Count());
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product is null)
            {
                _logger.LogWarning("Produto não encontrado. Id={ProductId}", id);
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Falha de validação ao criar produto. Erros={Errors}",
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return ValidationProblem(ModelState);
            }

            product.Id = Guid.NewGuid();
            await _productRepository.AddAsync(product);

            _logger.LogInformation("Produto criado com sucesso. Id={ProductId}, Nome={Name}, Preco={Price}",
                product.Id, product.Name, product.Price);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                _logger.LogWarning("Id do caminho difere do corpo da requisição ao atualizar produto. RouteId={RouteId}, BodyId={BodyId}",
                    id, product.Id);
                return BadRequest("Id do caminho difere do corpo da requisição.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Falha de validação ao atualizar produto. Id={ProductId}", id);
                return ValidationProblem(ModelState);
            }

            var existing = await _productRepository.GetByIdAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Tentativa de atualizar produto inexistente. Id={ProductId}", id);
                return NotFound();
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            existing.QuantityInStock = product.QuantityInStock;

            await _productRepository.UpdateAsync(existing);
            _logger.LogInformation("Produto atualizado com sucesso. Id={ProductId}", id);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _productRepository.GetByIdAsync(id);
            if (existing is null)
            {
                _logger.LogWarning("Tentativa de excluir produto inexistente. Id={ProductId}", id);
                return NotFound();
            }
            await _productRepository.DeleteAsync(id);
            _logger.LogInformation("Produto excluído com sucesso. Id={ProductId}", id);
            return NoContent();
        }
    }
}

