using System.Collections.Generic;
using System.Linq;
using Aditi.GIS.ARM.DataAccess.Entities;
using Aditi.GIS.Foundation.DataAccess.Entities;
using NHibernate.Linq;

namespace Aditi.GIS.ARM.DataAccess.Repository
{
    public class JoinTableRepository<TLeft, TRight> : CommonRepository<TLeft>
        where TLeft : EntityModelBase
        where TRight : EntityModelBase
    {
        public JoinTableRepository()
            : base()
        {
        }

        public IEnumerable<TLeft> GetLeft(long rid)
        {
            return this.repository.Session.Query<MToMTableModel<TLeft, TRight>>()
                .Where(x => x.IsActive == true 
                    && x.LeftItem.IsActive == true 
                    && x.RightItem.Id == rid)
                .Select(x => x.LeftItem);
        }

        public IEnumerable<TRight> GetRight(long rid)
        {
            return this.repository.Session.Query<MToMTableModel<TLeft, TRight>>()
                .Where(x => x.IsActive == true
                    && x.RightItem.IsActive == true
                    && x.LeftItem.Id == rid)
                .Select(x => x.RightItem);
        }

        /// <summary>
        /// where Right Table is Versionable
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public IEnumerable<TLeft> GetVLeft(long rid)
        {
            return this.repository.Session.Query<MToMVersionableTableModel<TLeft, TRight>>()
                .Where(x => x.IsActive == true
                    && x.LeftItem.IsActive == true
                    && x.RightItem.Id == rid)
                .Select(x => x.LeftItem);
        }


        /// <summary>
        /// where Left Table is Versionable
        /// </summary>
        /// <param name="rid"></param>
        /// <returns></returns>
        public IEnumerable<TRight> GetVRight(long rid)
        {
            return this.repository.Session.Query<MToMVersionableTableModel<TLeft, TRight>>()
                .Where(x => x.IsActive == true
                    && x.RightItem.IsActive == true
                    && x.LeftItem.Id == rid)
                .Select(x => x.RightItem);
        }
    }
}
