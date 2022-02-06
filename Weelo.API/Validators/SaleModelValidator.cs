using FluentValidation;
using Weelo.API.Models;

namespace Weelo.API.Validators
{
    /// <summary>
    /// Property validation configuration
    /// </summary>
    public class SaleModelValidator : AbstractValidator<SaleModel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SaleModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .Length(0, 250)
                .WithMessage("Name must have a length from 0 to 250")
                ;
        }
    }
}
