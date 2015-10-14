using System.Collections.Generic;
using System.Linq;
using Aditi.GIS.ARM.DataAccess.Entities;
using Aditi.GIS.Foundation.Repository;
using NHibernate.Linq;

namespace Aditi.GIS.ARM.DataAccess.Repository
{
    public class EmployeeRepository : NHibernateRepositoryBase<EmployeeModel>, IRepository<EmployeeModel>
    {
        public EmployeeRepository()
            : base(NHibernateHelper.OpenSession())
        {
        }

        public IEnumerable<EmployeeModel> GetEmployees(string empid)
        {
            return this.Session.Query<EmployeeModel>().Where(x => x.EmpId == empid).Select(x => x);
        }
    }
}
