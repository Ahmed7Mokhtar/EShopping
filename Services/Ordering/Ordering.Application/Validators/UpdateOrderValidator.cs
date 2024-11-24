using FluentValidation;
using Ordering.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Application.Validators
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderDTO>
    {
        public UpdateOrderValidator()
        {
            RuleFor(dto => dto.Id)
                .NotEmpty()
                .WithMessage("{Id} is required!");

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

            RuleFor(dto => dto.Email)
                .NotEmpty()
                .WithMessage("{Email} is required!");

            RuleFor(dto => dto.FirstName)
                .NotEmpty()
                .WithMessage("{FirstName} is required!")
                .MaximumLength(200)
                .WithMessage("{FirstName} must not exceed 200 characters");

            RuleFor(dto => dto.LastName)
                .NotEmpty()
                .WithMessage("{LastName} is required!")
                .MaximumLength(200)
                .WithMessage("{LastName} must not exceed 200 characters");
        }
    }
}
