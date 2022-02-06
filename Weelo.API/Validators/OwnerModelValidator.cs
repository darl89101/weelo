using FluentValidation;
using Weelo.API.Models;

namespace Weelo.API.Validators
{
    /// <summary>
    /// Owner validator configuration
    /// </summary>
    public class OwnerModelValidator : AbstractValidator<OwnerModel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OwnerModelValidator()
        {
            RuleFor(x => x.DocumentNumber)
                .NotEmpty()
                .WithMessage("Owner DocumentNumber is required")
                .Length(5, 20)
                .WithMessage("Owner DocumentNumber must have a length from 5 to 20")
                ;
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Owner Name is required")
                .Length(0, 250)
                .WithMessage("Owner Name must have a length from 0 to 250")
                ;
            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Owner Address is required")
                .Length(0, 250)
                .WithMessage("Owner Address must have a length from 0 to 250")
                ;
            RuleFor(x => x.BirthDay)
                .NotEqual(new DateTime())
                .WithMessage("Owner BirthDate is required")
                ;
        }
    }
}
