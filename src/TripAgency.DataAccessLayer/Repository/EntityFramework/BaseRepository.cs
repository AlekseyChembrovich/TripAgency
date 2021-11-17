using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TripAgency.DataAccessLayer.Repository.EntityFramework
{
    public class BaseRepository<T> : IRepository<T> where T : class, new()
    {
        protected readonly DatabaseContext Context;

        public BaseRepository(DatabaseContext context)
        {
            Context = context;
        }

        public int Insert(T entity)
        {
            if (entity is null) return 0;
            Context.Set<T>().Add(entity);
            try
            {
                var count = Context.SaveChanges();
                return count;
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return 0;
        }

        public int Update(T entity)
        {
            if (entity is null) return 0;
            Context.Entry(entity).State = EntityState.Detached;
            Context.Set<T>().Attach(entity);
            Context.Set<T>().Update(entity);
            try
            {
                var count = Context.SaveChanges();
                return count;
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return 0;
        }

        public int Delete(T entity)
        {
            if (entity is null) return 0;
            Context.Set<T>().Attach(entity);
            Context.Set<T>().Remove(entity);
            try
            {
                var count = Context.SaveChanges();
                return count;
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return 0;
        }

        public IEnumerable<T> GetAll()
        {
            return Context.Set<T>().AsEnumerable();
        }

        public T GetById(int id)
        {
            var entity = Context.Set<T>().Find(id);
            if (entity is not null)
            {
                Context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public T GetModelByProperty(string value, string nameProperty)
        {
            var typeModel = typeof(T);
            var propertyInfo = typeModel.GetProperty(nameProperty);
            if (propertyInfo == null)
            {
                return null;
            }

            var listModels = Context.Set<T>().ToList();
            foreach (var model in listModels)
            {
                var valueProperty = propertyInfo.GetValue(model)?.ToString();
                if (valueProperty == null) continue;
                if (valueProperty.ToLower().Contains(value.ToLower()))
                {
                    return model;
                }
            }

            return null;
        }
    }
}
