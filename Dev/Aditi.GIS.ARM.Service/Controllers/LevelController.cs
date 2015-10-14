using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aditi.GIS.ARM.Contracts;
using Aditi.GIS.ARM.DataAccess.Entities;
using Aditi.GIS.ARM.DataAccess.Repository;
using Aditi.GIS.ARM.Service.Facade;
using Aditi.GIS.Foundation.Caching;
using Aditi.GIS.Foundation.Extensions;
using Aditi.GIS.Foundation.Mapper;
using Aditi.GIS.Foundation.Services.Contracts;

namespace Aditi.GIS.ARM.Service.Controllers
{
    [RoutePrefix("gis/arm/level")]
    public class LevelController : ApiController
    {
        private static ICacheObject cache = InMemoryCache.Instance;

        [Route("{id:long:min(1)}")]
        [HttpGet]
        public HttpResponseMessage GetLevel(long id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Level output = null;
            if (!TryGetItem(id, out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            output.SetDefaultLinks();
            Response<Level> result = new Response<Level>(output);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetLevels()
        {
            IEnumerable<Level> output = null;
            if (!TryGetAllItems(out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Level>> results = new Response<IEnumerable<Level>>(output);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id:long:min(1)}/employee")]
        [HttpGet]
        public HttpResponseMessage GetEmployees(long id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Employee> output = null;
            if (!TryGetItemEmployees(id, out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Employee>> results = new Response<IEnumerable<Employee>>(output);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }


        private bool TryGetItem(long id, out Level result)
        {
            string cacheKey = "aditi:gis:arm:data:level-" + id.ToString();
            if (!cache.TryGet(cacheKey, out result))
            {
                CommonService<LevelModel, Level> service = new CommonService<LevelModel, Level>();
                result = service.Get(id);
                if (result == null)
                {
                    result = null;
                    return false;
                }

                result.SetDefaultLinks();
                cache.Set(cacheKey, result);
            }

            return true;
        }

        private bool TryGetAllItems(out IEnumerable<Level> results)
        {
            string cacheKey = "aditi:gis:arm:data:level-all";
            if (!cache.TryGet(cacheKey, out results))
            {
                CommonService<LevelModel, Level> service = new CommonService<LevelModel, Level>();
                results = service.GetList();
                if (results.IsNullOrEmpty())
                {
                    results = null;
                    return false;
                }

                cache.Set(cacheKey, results);
            }

            return true;
        }

        private bool TryGetItemEmployees(long id, out IEnumerable<Employee> results)
        {
            string cacheKey = "aditi:gis:arm:data:level-" + id.ToString() + "-employee";
            if (!cache.TryGet(cacheKey, out results))
            {
                JoinTableRepository<EmployeeModel, LevelModel> repository = new JoinTableRepository<EmployeeModel, LevelModel>();
                IEnumerable<EmployeeModel> employees = repository.GetLeft(id);
                results = AutoMapper.MapCollection<EmployeeModel, Employee>(employees);
                if (results.IsNullOrEmpty())
                {
                    results = null;
                    return false;
                }

                cache.Set(cacheKey, results, InMemoryCache.OneDay);
            }

            return true;
        }
    }
}
