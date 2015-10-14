using System;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Aditi.GIS.ARM.ServiceHost
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterFilters(GlobalConfiguration.Configuration);
            RegisterMessageHandlers(GlobalConfiguration.Configuration);
            RegisterRoutes(GlobalConfiguration.Configuration);
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();
            GlobalConfiguration.Configuration.EnsureInitialized();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        private static void RegisterMessageHandlers(HttpConfiguration httpConfiguration)
        {
        }

        private static void RegisterFilters(HttpConfiguration config)
        {
        }

        private static void RegisterRoutes(HttpConfiguration config)
        {
        }
    }
}