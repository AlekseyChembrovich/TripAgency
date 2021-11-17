using System.Collections.Generic;

namespace TripAgency.DataAccessLayer.Repository
{
    public interface IRepository<T>
    {
        int Insert(T entity);

        int Update(T entity);
        
        int Delete(T entity);
        
        IEnumerable<T> GetAll();
        
        T GetById(int id);

        T GetModelByProperty(string value, string nameProperty);
    }
}
