using System;
using System.Collections.Generic;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.DataAccess.Entities;

namespace Aditi.GIS.Foundation.Repository
{
    public interface IRepository<T> where T : IEntityModelBase
    {
        T Get(long id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetBySpecification(ISpecification<T> specification);

        /// <summary>
        /// Not in scope
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Add(T entity);
        /// <summary>
        /// Not in scope
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        T Update(T entity);
        /// <summary>
        /// Not in scope
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);
    }
}
