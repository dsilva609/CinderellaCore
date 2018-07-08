using CinderellaCore.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
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
            AddOrUpdate(entity);
            _context.SaveChanges();
        }

        public T AddOrUpdate(T entity)
        {
            var entityEntry = _context.Entry(entity);

            var primaryKeyName = entityEntry.Context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                .Select(x => x.Name).Single();

            var primaryKeyField = entity.GetType().GetProperty(primaryKeyName);

            var t = typeof(T);
            if (primaryKeyField == null)
            {
                throw new Exception($"{t.FullName} does not have a primary key specified. Unable to exec AddOrUpdate call.");
            }
            var keyVal = primaryKeyField.GetValue(entity);
            var dbVal = _dbSet.Find(keyVal);

            if (dbVal != null)
            {
                _context.Entry(dbVal).CurrentValues.SetValues(entity);
                _dbSet.Update(dbVal);

                entity = dbVal;
            }
            else
            {
                _dbSet.Add(entity);
            }

            return entity;
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