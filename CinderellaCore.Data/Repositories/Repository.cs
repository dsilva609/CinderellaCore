using CinderellaCore.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CinderellaCore.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly CinderellaCoreContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(CinderellaCoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public IQueryable<T> GetAll() => _dbSet.AsQueryable();

        public T GetByID(int id, string userID) => _dbSet.Find(id);

        public void Edit(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Set<T>().Update(entity);
            _context.SaveChanges();
        }

        public void Edit(List<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            _context.SaveChanges();
        }

        public void Delete(int id, string userID)
        {
            var entry = _dbSet.Find(id);
            _dbSet.Remove(entry);
            _context.SaveChanges();
        }

        public int GetCount() => _dbSet.Count();
    }
}