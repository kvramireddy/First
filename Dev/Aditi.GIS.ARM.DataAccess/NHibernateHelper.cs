using System.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Aditi.GIS.ARM.DataAccess
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        private static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)

                    InitializeSessionFactory();
                return _sessionFactory;
            }
        }

        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(PostgreSQLConfiguration.PostgreSQL82
                .ConnectionString(ConfigurationManager.ConnectionStrings["armgisdb"].ConnectionString)
                .ShowSql())
                .Mappings(m =>
                          m.FluentMappings
                              .AddFromAssemblyOf<Aditi.GIS.ARM.DataAccess.Entities.LocationModel>())
                .Cache(x => 
                //Configuring the SysCacheProvider class 
                //as the Second Level Cache Provider.
                       x.UseQueryCache()
                       .ProviderClass("NHibernate.Caches.SysCache.SysCacheProvider, NHibernate.Caches.SysCache"))
                .BuildSessionFactory();
        }

        public static ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
