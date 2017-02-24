namespace Sib.Core.Domain
{
    public abstract class ServiceWork
    {
        public abstract ServiceWorkType WorkType { get; }

        public abstract string Name { get; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public class Arruada : ServiceWork
    {
        public override string Name { get; } = nameof(Arruada);

        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Arruada;
    }

    public class Missa : ServiceWork
    {
        public override string Name { get; } = nameof(Missa);

        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Missa;
    }

    public class Procissao : ServiceWork
    {
        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Procissao;

        public override string Name { get; } = "Procissão";
    }

    public class Concerto : ServiceWork
    {
        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Concerto;

        public override string Name { get; } = nameof(Concerto);
    }

    public class Peditorio : ServiceWork
    {
        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Peditorio;

        public override string Name { get; } = "Peditório";
    }

    public class Entrega : ServiceWork
    {
        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Entrega;

        public override string Name { get; } = nameof(Entrega);
    }

    public class Outro : ServiceWork {
        public override ServiceWorkType WorkType { get; } = ServiceWorkType.Outro;

        public override string Name { get; } = nameof(Outro);
    }
}