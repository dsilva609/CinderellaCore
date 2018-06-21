using System;

namespace CinderellaCore.Data.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;

        void SaveChanges();
    }
}