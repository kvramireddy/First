using System.Collections.Generic;
using System.Linq;

namespace Aditi.GIS.Foundation.Extensions
{
    public static class Common
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || collection.Count() == 0;
        }
    }
}
