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
    [RoutePrefix("gis/arm/project")]
    public class ProjectController : ApiController
    {
        private static ICacheObject cache = InMemoryCache.Instance;

        [Route("{id:long:min(1)}")]
        [HttpGet]
        public HttpResponseMessage GetProject(long id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Project output = null;
            if (!TryGetItem(id, out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            output.SetDefaultLinks();
            Response<Project> result = new Response<Project>(output);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetProjects()
        {
            IEnumerable<Project> output = null;
            if (!TryGetAllItems(out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Project>> results = new Response<IEnumerable<Project>>(output);
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

        private bool TryGetItem(long id, out Project result)
        {
            string cacheKey = "aditi:gis:arm:data:project-" + id.ToString();
            if (!cache.TryGet(cacheKey, out result))
            {
                CommonService<ProjectModel, Project> service = new CommonService<ProjectModel, Project>();
                result = service.Get(id);
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

        private bool TryGetAllItems(out IEnumerable<Project> results)
        {
            string cacheKey = "aditi:gis:arm:data:project-all";
            if (!cache.TryGet(cacheKey, out results))
            {
                CommonService<ProjectModel, Project> service = new CommonService<ProjectModel, Project>();
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

        private bool TryGetItemEmployees(long id, out IEnumerable<Employee> results)
        {
            string cacheKey = "aditi:gis:arm:data:project-" + id.ToString() + "-employee";
            if (!cache.TryGet(cacheKey, out results))
            {
                JoinTableRepository<EmployeeModel, ProjectModel> repository = new JoinTableRepository<EmployeeModel, ProjectModel>();
                IEnumerable<EmployeeModel> employees = repository.GetVLeft(id);
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
