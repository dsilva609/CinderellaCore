using CinderellaCore.Data.Context;
using CinderellaCore.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CinderellaCoreContext _context;
        private readonly Dictionary<Type, object> _repositories;
        private bool _disposed;

        public UnitOfWork(GlobalSettings settings)
        {
            _context = new CinderellaCoreContext(new DbContextOptions<CinderellaCoreContext>(), settings);
            _repositories = new Dictionary<Type, object>();
            _disposed = false;
        }

        public IRepository<T1> GetRepository<T1>() where T1 : class
        {
            if (_repositories.Keys.Contains(typeof(T1))) return _repositories[typeof(T1)] as IRepository<T1>;

            var repository = new Repository<T1>(_context);
            _repositories.Add(typeof(T1), repository);

            return repository;
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
            _context.Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing) _context.Dispose();

            _disposed = true;
        }

        ~UnitOfWork() => _context.Dispose();
    }
}