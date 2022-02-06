using FluentValidation;
using Weelo.API.Models;

namespace Weelo.API.Validators
{
    /// <summary>
    /// Property validation configuration
    /// </summary>
    public class PropertyModelValidator : AbstractValidator<PropertyModel>
    { 
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .Length(0, 250)
                .WithMessage("Name must have a length from 0 to 250")
                ;
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required")
                .Length(0, 250)
                .WithMessage("Address must have a length from 0 to 250")
                ;
            RuleFor(x => x.Owner)
                .NotEmpty()
                .WithMessage("Owner is required")
                .SetValidator(new OwnerModelValidator())
                ;
        }
    }
}
