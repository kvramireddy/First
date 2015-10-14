using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aditi.GIS.ARM.Contracts;
using Aditi.GIS.ARM.DataAccess.Entities;
using Aditi.GIS.ARM.DataAccess.Repository;
using Aditi.GIS.ARM.Service.Facade;
using Aditi.GIS.Foundation.Caching;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.Foundation.Extensions;
using Aditi.GIS.Foundation.Mapper;
using Aditi.GIS.Foundation.Services.Contracts;

namespace Aditi.GIS.ARM.Service.Controllers
{
    [RoutePrefix("gis/arm/employee")]
    public class EmployeeController : ApiController
    {
        private static ICacheObject cache = InMemoryCache.Instance;

        [Route("{id}")]
        [HttpGet]
        public HttpResponseMessage GetEmployee(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Employee output = null;
            if (!TryGetItem(id, out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            output.SetDefaultLinks();
            Response<Employee> result = new Response<Employee>(output);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("{id}/location")]
        [HttpGet]
        public HttpResponseMessage GetLocation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Location> target = null;
            if (!TryGetEmployeeParts<Location, LocationModel>("location", id, out target))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Location>> results = new Response<IEnumerable<Location>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id}/skillset")]
        [HttpGet]
        public HttpResponseMessage GetSkillset(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Skillset> target = null;
            if (!TryGetEmployeeParts<Skillset, SkillsetModel>("skillset", id, out target))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Skillset>> results = new Response<IEnumerable<Skillset>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id}/project")]
        [HttpGet]
        public HttpResponseMessage GetProject(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Project> target = null;
            if (!TryGetEmployeeVParts<Project, ProjectModel>("project", id, out target))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Project>> results = new Response<IEnumerable<Project>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id}/stream")]
        [HttpGet]
        public HttpResponseMessage GetStream(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Stream> target = null;
            if (!TryGetEmployeeParts<Stream, StreamModel>("stream", id, out target))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Stream>> results = new Response<IEnumerable<Stream>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id}/level")]
        [HttpGet]
        public HttpResponseMessage GetLevel(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Level> target = null;
            if (!TryGetEmployeeParts<Level, LevelModel>("level", id, out target))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Level>> results = new Response<IEnumerable<Level>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetEmployees()
        {
            IEnumerable<Employee> output = null;
            if (!TryGetAllItems(out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Employee>> results = new Response<IEnumerable<Employee>>(output);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        private bool TryGetItem(string id, out Employee result)
        {
            string cacheKey = "aditi:gis:arm:data:employee-" + id.ToString();
            if (!cache.TryGet(cacheKey, out result))
            {
                EmployeeRepository employeeService = new EmployeeRepository();
                IEnumerable<EmployeeModel> employee = employeeService.GetEmployees(id);
                result = AutoMapper.Map<EmployeeModel, Employee>(employee.FirstOrDefault());
                if (result == null)
                {
                    result = null;
                    return false;
                }

                result.SetDefaultLinks();
                cache.Set(cacheKey, result, InMemoryCache.OneDay);
            }

            return true;
        }

        private bool TryGetAllItems(out IEnumerable<Employee> results)
        {
            string cacheKey = "aditi:gis:arm:data:employee-all";
            if (!cache.TryGet(cacheKey, out results))
            {
                CommonService<EmployeeModel, Employee> service = new CommonService<EmployeeModel, Employee>();
                results = service.GetList();
                if (results.IsNullOrEmpty())
                {
                    results = null;
                    return false;
                }

                cache.Set(cacheKey, results, InMemoryCache.OneDay);
            }

            return true;
        }

        private bool TryGetEmployeeParts<T, TModel>(string keyPart, string id, out IEnumerable<T> results)
            where TModel : EntityModelBase
            where T : DataContractBase, new()
        {
            string cacheKey = "aditi:gis:arm:data:employee-" + id + "-" + keyPart;
            if (!cache.TryGet(cacheKey, out results))
            {
                EmployeeTableJoinRepository<TModel> repository = new EmployeeTableJoinRepository<TModel>();
                IEnumerable<TModel> employees = repository.Get(id);
                results = AutoMapper.MapCollection<TModel, T>(employees);
                if (results.IsNullOrEmpty())
                {
                    results = null;
                    return false;
                }

                cache.Set(cacheKey, results, InMemoryCache.OneDay);
            }

            return true;
        }

        private bool TryGetEmployeeVParts<T, TModel>(string keyPart, string id, out IEnumerable<T> results)
            where TModel : EntityModelBase
            where T : DataContractBase, new()
        {
            string cacheKey = "aditi:gis:arm:data:employee-" + id + "-" + keyPart;
            if (!cache.TryGet(cacheKey, out results))
            {
                EmployeeTableJoinRepository<TModel> repository = new EmployeeTableJoinRepository<TModel>();
                IEnumerable<TModel> employees = repository.GetV(id);
                results = AutoMapper.MapCollection<TModel, T>(employees);
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
