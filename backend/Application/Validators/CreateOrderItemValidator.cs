using Application.DTOs.Create;
using FluentValidation;

namespace Application.Validators
{
    public class CreateOrderItemValidator : AbstractValidator<CreateOrderItemDto>
    {
        public CreateOrderItemValidator()
        {
            RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("O ID do produto é obrigatório.");

            RuleFor(i => i.Quantity)
                .GreaterThan(0)
                .WithMessage("A quantidade de cada produto deve ser maior que zero.");
        }
    }
}
