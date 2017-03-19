namespace Sib.Core.Domain
{
    using System;
    using System.Collections.Generic;

    using FluentValidation.Attributes;

    using MongoDB.Driver.GeoJsonObjectModel;

    using Sib.Core.Validators;

    public class Service : BaseDocument
    {
        public string Location { get; set; }

        public GeoJsonPoint<GeoJson2DCoordinates> Coordinates { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public IList<string> Work { get; set; } = new List<string>();
    }
}