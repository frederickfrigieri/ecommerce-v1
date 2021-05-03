using System;
using ECommerce.Core.DomainObjects;

namespace ECommerce.Core.Data
{
    public interface IRepository<T> : IDisposable where T : Entity, IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
