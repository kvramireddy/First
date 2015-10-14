using System;
using System.Collections.Generic;
using Aditi.GIS.ARM.Shared;
using Aditi.GIS.Foundation.DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class EmployeeModel : EntityModelBase, IEmployee
    {
        public virtual string EmpId { get; set; }
        public virtual string EmployeeFID { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string MiddleName { get; set; }

        // public virtual string Address1 { get; set; }
        // public virtual string Address2 { get; set; }
        // public virtual string Address3 { get; set; }

        public virtual DateTimeOffset JoiningDate { get; set; }
        public virtual DateTimeOffset RelivedOn { get; set; }

        // public virtual List<LocationModel> Location { get; set; }
        // public virtual List<SkillsetModel> Skillsets { get; set; }
        // public virtual List<ProjectModel> Project { get; set; }
        // public virtual List<LevelModel> Level { get; set; }
        // public virtual List<StreamModel> Stream { get; set; }
        public override bool Equals(object obj)
        {
            if (obj is IEmployee
                && this.EmpId != null
                && ((IEmployee)obj).EmpId != null)
            {
                return this.EmpId.Equals(((IEmployee)obj).EmpId);
            }
            else
            {
                return base.Equals(obj);
            }
        }
    }

    public class EmployeeMapper : ClassMap<EmployeeModel>
    {
        public EmployeeMapper()
        {
            this.Id(x => x.EmpId, "ID");
            this.Map(x => x.EmployeeFID, "emprefid");
            this.Map(x => x.FirstName, "firstname");
            this.Map(x => x.LastName, "lastname");
            this.Map(x => x.MiddleName, "middlename");
            this.Map(x => x.JoiningDate, "joiningdate");
            this.Map(x => x.RelivedOn, "relivedon");
            this.Map(x => x.IsActive, "IsActive");
            this.Table("employee");
            this.Schema("gis");
        }
    }
}
