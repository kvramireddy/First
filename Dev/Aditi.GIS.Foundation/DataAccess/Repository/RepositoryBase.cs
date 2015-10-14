using System;
using System.Collections.Generic;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.DataAccess.Entities;

namespace Aditi.GIS.Foundation.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T> 
        where T : IEntityModelBase
    {
        public abstract T Get(long id);
        public abstract IEnumerable<T> GetAll();
        public abstract IEnumerable<T> GetBySpecification(ISpecification<T> specification);

        public virtual T Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual bool Delete(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
