namespace Sib.Core.Validators
{
    using System;

    using FluentValidation;

    using Sib.Core.Domain;

    public class ServiceValidator : AbstractValidator<Service>
    {
        public ServiceValidator()
        {
            this.RuleFor(s => s.Date).ExclusiveBetween(new DateTime(2016, 10, 1), new DateTime(2017, 11, 1));
            this.RuleFor(s => s.Location).NotEmpty();
        }

    }
}
