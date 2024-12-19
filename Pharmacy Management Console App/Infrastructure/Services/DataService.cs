using PharmacyManagement.Core.Interfaces;
using PharmacyManagement.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;

namespace PharmacyManagement.Infrastructure.Services
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _context;

        public DataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return _context.Set<T>().ToList();
        }

        public T GetById<T>(int id) where T : class
        {
            return _context.Set<T>().Find(id);
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}