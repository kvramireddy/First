using System;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.ARM.Shared;
using FluentNHibernate.Mapping;

namespace Aditi.GIS.ARM.DataAccess.Entities
{
    public class StreamModel : VersionableEntityModelBase, IStream
    {
        public virtual string ShortName { get; set; }
        public virtual string LongName { get; set; }
        public virtual string Description { get; set; }
    }

    public class StreamMapper : ClassMap<StreamModel>
    {
        public StreamMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.LongName, "Name");
            this.Map(x => x.IsActive, "IsActive");
            this.Table("stream");
            this.Schema("gis");
        }
    }

    public class EmployeeStreamMapper : ClassMap<MToMTableModel<EmployeeModel, StreamModel>>
    {
        public EmployeeStreamMapper()
        {
            this.Id(x => x.Id, "ID");
            this.Map(x => x.IsActive, "IsActive");
            this.References(x => x.LeftItem, "empid");
            this.References(x => x.RightItem, "streamid");
            this.Table("empstream");
            this.Schema("gis");
        }
    }
}
