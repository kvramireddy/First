using System.Collections.Generic;
using System.Linq;
using Aditi.GIS.ARM.DataAccess.Entities;
using Aditi.GIS.Foundation.DataAccess.Entities;
using NHibernate.Linq;

namespace Aditi.GIS.ARM.DataAccess.Repository
{
    public class EmployeeTableJoinRepository<T> : CommonRepository<T>
        where T : EntityModelBase
    {
        public EmployeeTableJoinRepository()
            : base()
        {
        }

        public IEnumerable<T> Get(string empid)
        {
            return this.repository.Session.Query<MToMTableModel<EmployeeModel, T>>()
                .Where(x => x.IsActive == true
                    && x.RightItem.IsActive == true
                    && x.LeftItem.EmpId == empid)
                .Select(x => x.RightItem);
        }

        public IEnumerable<T> GetV(string empid)
        {
            return this.repository.Session.Query<MToMVersionableTableModel<EmployeeModel, T>>()
                .Where(x => x.IsActive == true
                    && x.RightItem.IsActive == true
                    && x.LeftItem.EmpId == empid)
                .Select(x => x.RightItem);
        }
    }
}
