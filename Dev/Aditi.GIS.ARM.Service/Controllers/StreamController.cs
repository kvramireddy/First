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
    [RoutePrefix("gis/arm/stream")]
    public class StreamController : ApiController
    {
        private static ICacheObject cache = InMemoryCache.Instance;

        [Route("{id:long:min(1)}")]
        [HttpGet]
        public HttpResponseMessage GetStream(long id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Stream stream = null;
            if (!TryGetItem(id, out stream))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<Stream> result = new Response<Stream>(stream);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetStreams()
        {
            IEnumerable<Stream> streams = null;
            if (!TryGetAllItems(out streams))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Stream>> results = new Response<IEnumerable<Stream>>(streams);
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

            IEnumerable<Employee> target = null;
            if (!TryGetItemEmployees(id, out target))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Employee>> results = new Response<IEnumerable<Employee>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }


        private bool TryGetItem(long id, out Stream result)
        {
            string cacheKey = "aditi:gis:arm:data:stream-" + id.ToString();
            if (!cache.TryGet(cacheKey, out result))
            {
                CommonService<StreamModel, Stream> service = new CommonService<StreamModel, Stream>();
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

        private bool TryGetAllItems(out IEnumerable<Stream> results)
        {
            string cacheKey = "aditi:gis:arm:data:stream-all";
            if (!cache.TryGet(cacheKey, out results))
            {
                CommonService<StreamModel, Stream> service = new CommonService<StreamModel, Stream>();
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
            string cacheKey = "aditi:gis:arm:data:stream-" + id.ToString() + "-employee";
            if (!cache.TryGet(cacheKey, out results))
            {
                JoinTableRepository<EmployeeModel, StreamModel> repository = new JoinTableRepository<EmployeeModel, StreamModel>();
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
