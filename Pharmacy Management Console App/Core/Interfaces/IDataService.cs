using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyManagement.Core.Interfaces
{
    public interface IDataService
    {
        IEnumerable<T> GetAll<T>() where T : class;
        T GetById<T>(int id) where T : class;
        void Add<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void SaveChanges();
    }

}
