using FluentValidation;
using Weelo.API.Models;

namespace Weelo.API.Validators
{
    /// <summary>
    /// Property validation configuration
    /// </summary>
    public class PropertyImageModelValidator : AbstractValidator<PropertyImageModel>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PropertyImageModelValidator()
        {
            RuleFor(x => x.Files)
                .NotEmpty()
                .WithMessage("File is required")
                .Must(m => m.All(x => Domain.Commons.Validators.IsImageFormat(x)))                
                .WithMessage("The file does not contain a correct image format.")
                ;
        }
    }
}
