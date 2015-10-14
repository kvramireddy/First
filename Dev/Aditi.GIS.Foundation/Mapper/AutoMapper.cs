using System.Collections.Generic;
using Aditi.GIS.Foundation.Shared;

namespace Aditi.GIS.Foundation.Mapper
{
    public static class AutoMapper
    {
        public static TTo Map<TFrom, TTo>(TFrom from)
            where TFrom : IDto
            where TTo : IDto, new()
        {
            TTo to = default(TTo);
            if (from != null)
            {
                to = new TTo();
                to.Transfer(from);
            }

            return to;
        }

        public static IEnumerable<TTo> MapCollection<TFrom, TTo>(IEnumerable<TFrom> list)
            where TFrom : IDto
            where TTo : IDto, new()
        {
            IList<TTo> output = null;
            if (list != null)
            {
                output = new List<TTo>();
                foreach (var from in list)
                {
                    output.Add(Map<TFrom, TTo>(from));
                }
            }

            return output;
        }
    }
}
