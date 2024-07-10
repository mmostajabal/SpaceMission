using FluentValidation;
using SpaceMissionShared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceMissionShared.Validation
{
    public  class TemperatureValidator : AbstractValidator<TemperatureDTO>
    {
        public TemperatureValidator() {
            RuleFor(x => x.TemperatureCelsius).NotEmpty().NotNull().WithMessage("Points must be greater than zero.");
            RuleFor(x => x.Timestamp).NotEmpty().NotNull().WithMessage("Points must be greater than zero.");
        }
    }
}
