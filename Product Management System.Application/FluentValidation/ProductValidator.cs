using FluentValidation;
using Product_Management_System.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_Management_System.Application.FluentValidation
{
    public class ProductValidator : AbstractValidator<CreateOrUpdateProductDtos>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must be less than 100 characters");

            RuleFor(p => p.Description)
                .MaximumLength(500).WithMessage("Description must be less than 500 characters");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");


        }
    }
}
