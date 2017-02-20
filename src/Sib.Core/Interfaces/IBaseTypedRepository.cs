namespace Sib.Core.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IBaseTypedRepository<TDoc>
    where TDoc : IDocument
    {
        Task Insert(TDoc document);

        Task Delete(TDoc document);

        Task Update(TDoc document);

        Task<TDoc> FindById(string id);
    }
}