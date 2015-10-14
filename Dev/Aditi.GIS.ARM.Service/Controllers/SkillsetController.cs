using System;
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
using Aditi.GIS.Foundation.Extensions;
using Aditi.GIS.Foundation.Mapper;
using Aditi.GIS.Foundation.Services.Contracts;

namespace Aditi.GIS.ARM.Service.Controllers
{
    [RoutePrefix("gis/arm/skillset")]
    public class SkillsetController : ApiController
    {
        private static ICacheObject cache = InMemoryCache.Instance;

        [Route("{id:long:min(1)}")]
        [HttpGet]
        public HttpResponseMessage GetSkillset(long id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Skillset output = null;
            if (!TryGetItem(id, out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<Skillset> result = new Response<Skillset>(output);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetSkillsets()
        {
            IEnumerable<Skillset> output = null;
            if (!TryGetAllItems(out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Skillset>> results = new Response<IEnumerable<Skillset>>(output);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id:long:min(1)}/employee")]
        [HttpGet]
        public HttpResponseMessage GetEmployees(long id, [FromUri]long[] loc = null)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            IEnumerable<Employee> output = null;
            IEnumerable<EmployeeModel> employees = null;
            if (!loc.IsNullOrEmpty())
            {
                JoinTableRepository<EmployeeModel, SkillsetModel> repository = new JoinTableRepository<EmployeeModel, SkillsetModel>();
                IEnumerable<string> employeeIds = repository.GetLeft(id).Select(x => x.EmpId).ToList();
                CommonRepository<MToMTableModel<EmployeeModel, LocationModel>> repositoryEmpLoc = new CommonRepository<MToMTableModel<EmployeeModel, LocationModel>>();
                employees = repositoryEmpLoc.GetBySpecification(new Specification<MToMTableModel<EmployeeModel, LocationModel>>(x => x.LeftItem.IsActive == true && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId) && loc.Contains(x.RightItem.Id))).Select(x => x.LeftItem);
                output = AutoMapper.MapCollection<EmployeeModel, Employee>(employees);
            }
            else
            {
                TryGetItemEmployees(id, out output);
            }

            if (output.IsNullOrEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Employee>> results = new Response<IEnumerable<Employee>>(output);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        [Route("{id:long:min(1)}/locations")]
        [HttpGet]
        public HttpResponseMessage GetLocations(long id, [FromUri]string groupBy)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            if (string.IsNullOrEmpty(groupBy) || !string.Equals(groupBy, "emp", StringComparison.OrdinalIgnoreCase))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            List<Location> locations = null;
            JoinTableRepository<EmployeeModel, SkillsetModel> repository = new JoinTableRepository<EmployeeModel, SkillsetModel>();
            IEnumerable<EmployeeModel> employees = repository.GetLeft(id);
            IEnumerable<string> employeeIds = employees.Select(x => x.EmpId);
            if (!employeeIds.IsNullOrEmpty())
            {
                CommonRepository<MToMTableModel<EmployeeModel, LocationModel>> repositoryEmpLoc = new CommonRepository<MToMTableModel<EmployeeModel, LocationModel>>();
                var op = repositoryEmpLoc.GetBySpecification(new Specification<MToMTableModel<EmployeeModel, LocationModel>>(x => x.LeftItem.IsActive == true && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId)));
                var tbl = from t1 in op
                          group t1 by t1.RightItem into newGroup
                          select new { Loc = newGroup.Key, EmpCount = newGroup.Count() };
                locations = new List<Location>();
                foreach (var item in tbl)
                {
                    Location loc = AutoMapper.Map<LocationModel, Location>(item.Loc);
                    loc.ExtendedProperties = new Dictionary<string, string>()
                    {
                        { "emp-count", item.EmpCount.ToString()} // string.Join(",", item.Select(x => x.LeftItem.EmpId)) }
                    };

                    locations.Add(loc);
                }
            }

            IEnumerable<Location> target = locations; // AutoMapper.MapCollection<LocationModel, Location>(locations);
            if (target.IsNullOrEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Location>> results = new Response<IEnumerable<Location>>(target);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }


        private bool TryGetItem(long id, out Skillset result)
        {
            string cacheKey = "aditi:gis:arm:data:skillset-" + id.ToString();
            if (!cache.TryGet(cacheKey, out result))
            {
                CommonService<SkillsetModel, Skillset> service = new CommonService<SkillsetModel, Skillset>();
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

        private bool TryGetAllItems(out IEnumerable<Skillset> results)
        {
            string cacheKey = "aditi:gis:arm:data:skillset-all";
            if (!cache.TryGet(cacheKey, out results))
            {
                CommonService<SkillsetModel, Skillset> service = new CommonService<SkillsetModel, Skillset>();
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
            string cacheKey = "aditi:gis:arm:data:skillset-" + id.ToString() + "-employee";
            if (!cache.TryGet(cacheKey, out results))
            {
                JoinTableRepository<EmployeeModel, SkillsetModel> repository = new JoinTableRepository<EmployeeModel, SkillsetModel>();
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
