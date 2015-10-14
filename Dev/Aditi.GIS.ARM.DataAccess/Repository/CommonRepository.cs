using System.Collections.Generic;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.Foundation.Repository;

namespace Aditi.GIS.ARM.DataAccess.Repository
{
    public class CommonRepository<T> : RepositoryBase<T>, IRepository<T>
        where T : IEntityModelBase
    {
        protected NHibernateRepositoryBase<T> repository;

        public CommonRepository()
        {
            this.repository = new NHibernateRepositoryBase<T>(NHibernateHelper.OpenSession());
        }

        public override T Get(long id)
        {
            return this.repository.Get(id);
        }

        public override IEnumerable<T> GetAll()
        {
            return this.repository.GetAll();
        }

        public override IEnumerable<T> GetBySpecification(ISpecification<T> specification)
        {
            return this.repository.GetBySpecification(specification);
        }

        /// <summary>
        /// Not in scope
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Add(T entity)
        {
            return this.repository.Add(entity);
        }

        /// <summary>
        /// Not in scope
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual T Update(T entity)
        {
            return this.repository.Update(entity);
        }

        /// <summary>
        /// Not in scope
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {
            return this.repository.Delete(entity);
        }
    }
}
