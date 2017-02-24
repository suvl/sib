namespace Sib.Core.Dto
{
    using System;
    using System.Collections.Generic;

    using FluentValidation.Attributes;

    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using Sib.Core.Domain;
    using Sib.Core.Validators;

    [Validator(typeof(ServiceValidator)), DataContract]
    public class ServiceModel
    {
        [DataMember, Required(ErrorMessage = "A localização é necessária")]
        public string Location { get; set; }

        [DataMember, Required(ErrorMessage = "A data é obrigatória.")]
        public DateTime Date { get; set; }

        [DataMember]
        public string Start { get; set; }

        [DataMember]
        public string End { get; set; }

        [DataMember]
        public IList<ServiceWork> Work { get; set; } = new List<ServiceWork>();

        [DataMember]
        public IReadOnlyList<ServiceWork> NewWorks { get; } = new List<ServiceWork>
                                                                  {
                                                                      new Arruada(),
                                                                      new Peditorio(),
                                                                      new Missa(),
                                                                      new Procissao(),
                                                                      new Concerto(),
                                                                      new Entrega(),
                                                                      new Outro()
                                                                  };

        [DataMember]
        public string Id { get; set; }
    }
}