using Application.DTOs.Create;
using FluentValidation;

namespace Application.Validators
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderValidator()
        {
            RuleFor(o => o.ClientId)
                .NotEmpty()
                .WithMessage("O ID do cliente é obrigatório para criar um pedido.");

            RuleFor(o => o.Items)
                .NotNull()
                .WithMessage("A lista de produtos não pode ser vazia.")
                .NotEmpty()
                .WithMessage("O pedido deve conter pelo menos um produto.");

            RuleForEach(o => o.Items).SetValidator(new CreateOrderItemValidator());
        }
    }

    
}
