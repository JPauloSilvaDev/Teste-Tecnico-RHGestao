using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(customers);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            customer.Id = Guid.NewGuid();
            await _customerRepository.AddAsync(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest("Id do caminho difere do corpo da requisição.");
            }

            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var existing = await _customerRepository.GetByIdAsync(id);
            if (existing is null)
            {
                return NotFound();
            }

            existing.Name = customer.Name;
            existing.Email = customer.Email;

            await _customerRepository.UpdateAsync(existing);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _customerRepository.GetByIdAsync(id);
            if (existing is null)
                return NotFound();
            await _customerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}

