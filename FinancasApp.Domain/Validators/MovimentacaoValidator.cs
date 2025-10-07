using FinancasApp.Domain.Entities;
using FluentValidation;
using System;

namespace FinancasApp.Domain.Validators
{
    public class MovimentacaoValidator : AbstractValidator<Movimentacao>
    {
        public MovimentacaoValidator()
        {
            RuleFor(m => m.Nome)
                .NotEmpty().WithMessage("O nome da movimentação é obrigatório.")
                .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.")
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

            RuleFor(m => m.Valor)
                .GreaterThan(0).WithMessage("O valor da movimentação deve ser maior que zero.");

            RuleFor(m => m.Data)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
                .WithMessage("A data da movimentação não pode ser no futuro.");

            RuleFor(m => m.Tipo)
                .IsInEnum()
                .WithMessage("O tipo de movimentação deve ser Receita ou Despesa.");
        }
    }
}