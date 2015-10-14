using System.Web;
using System.Web.Mvc;

namespace Aditi.GIS.ARM.Client
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}