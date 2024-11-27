using FluentValidation;
using Ordering.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class CheckoutOrderV2CommandValidator : AbstractValidator<CheckoutOrderV2DTO>
    {
        public CheckoutOrderV2CommandValidator()
        {
            RuleFor(dto => dto.UserName)
                .NotEmpty()
                .WithMessage("{UserName} is required!")
                .MaximumLength(70)
                .WithMessage("{UserName} must not exceed 70 characters");

            RuleFor(dto => dto.TotalPrice)
                .NotEmpty()
                .WithMessage("{TotalPrice} is required!")
                .GreaterThan(-1)
                .WithMessage("{TotalPrice} must be positive number");
        }
    }
}
