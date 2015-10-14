using System;
using System.Collections.Generic;
using System.Linq;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.DataAccess.Entities;
using NHibernate;
using NHibernate.Linq;

namespace Aditi.GIS.Foundation.Repository
{
    public class NHibernateRepositoryBase<T> : RepositoryBase<T>, IRepository<T>
        where T : IEntityModelBase
    {
        public NHibernateRepositoryBase(ISession session)
        {
            this.Session = session;
        }

        public ISession Session { get; private set; }

        public override T Get(long id)
        {
            return this.Session.Get<T>(id);
        }

        public override IEnumerable<T> GetAll()
        {
            return this.Session.Query<T>().Where(x => x.IsActive).Select(x => x);
        }

        public override IEnumerable<T> GetBySpecification(ISpecification<T> specification)
        {
            return this.Session.Query<T>().Where(x => x.IsActive).Where(specification.Predicate).Select(x => x);
        }
    }
}
