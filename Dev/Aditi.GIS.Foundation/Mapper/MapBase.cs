using System.Collections.Generic;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.Mapper
{
    public abstract class MapBase<TFrom, TTo> : IMap<TFrom, TTo>
        where TFrom : IDto
        where TTo : IDto, new()
    {
        public TTo Map(TFrom from)
        {
            TTo to = default(TTo);
            if (from != null)
            {
                to = new TTo();
                to.Transfer(from);
            }

            return to;
        }

        public IEnumerable<TTo> MapCollection(IEnumerable<TFrom> list)
        {
            IList<TTo> output = null;
            if (list != null)
            {
                output = new List<TTo>();
                foreach (var from in list)
                {
                    output.Add(this.Map(from));
                }
            }

            return output;
        }
    }

}
