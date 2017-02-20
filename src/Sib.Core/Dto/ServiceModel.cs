namespace Sib.Core.Dto
{
    using System;
    using System.Collections.Generic;

    using FluentValidation.Attributes;

    using System.ComponentModel.DataAnnotations;

    using Sib.Core.Domain;
    using Sib.Core.Validators;

    [Validator(typeof(ServiceValidator))]
    public class ServiceModel
    {
        [Required(ErrorMessage = "A localização é necessária")]
        public string Location { get; set; }

        [Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime Date { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public IList<ServiceWork> Work { get; set; } = new List<ServiceWork>();

        public string Id { get; set; }
    }
}