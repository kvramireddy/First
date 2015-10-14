using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Aditi.GIS.ARM.Contracts;
using Aditi.GIS.ARM.DataAccess;
using Aditi.GIS.ARM.DataAccess.Entities;
using Aditi.GIS.ARM.DataAccess.Repository;
using Aditi.GIS.ARM.Service.Facade;
using Aditi.GIS.Foundation.Caching;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.Extensions;
using Aditi.GIS.Foundation.Mapper;
using Aditi.GIS.Foundation.Repository;
using Aditi.GIS.Foundation.Services.Contracts;

namespace Aditi.GIS.ARM.Service.Controllers
{
    [RoutePrefix("gis/arm/location")]
    public class LocationController : ApiController
    {
        private static ICacheObject cache = InMemoryCache.Instance;

        [Route("{id:long:min(1)}")]
        [HttpGet]
        public HttpResponseMessage GetLocation(long id)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            Location output = null;
            if (!TryGetItem(id, out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<Location> result = new Response<Location>(output);
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("")]
        [HttpGet]
        public HttpResponseMessage GetLocations()
        {
            IEnumerable<Location> output = null;
            if (!TryGetAllItems(out output))
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Location>> results = new Response<IEnumerable<Location>>(output);
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

        [Route("{id:long:min(1)}/employeeutilization")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeUtilization(long id, [FromUri]long[] skill = null)
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            CustomData result = null;
            result = new CustomData();
            JoinTableRepository<EmployeeModel, LocationModel> repository = new JoinTableRepository<EmployeeModel, LocationModel>();
            IEnumerable<string> employeeIds = repository.GetLeft(id).Select(x => x.EmpId).ToList();
            CommonRepository<MToMTableModel<EmployeeModel, SkillsetModel>> repositoryEmpSkills = new CommonRepository<MToMTableModel<EmployeeModel, SkillsetModel>>();
            CommonRepository<MToMVersionableTableModel<EmployeeModel, ProjectModel>> repositoryEmpProjects = new CommonRepository<MToMVersionableTableModel<EmployeeModel, ProjectModel>>();
            if (skill.IsNullOrEmpty())
            {
                skill = repositoryEmpSkills.GetBySpecification(new Specification<MToMTableModel<EmployeeModel, SkillsetModel>>(x => x.LeftItem.IsActive == true && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId))).Select(x => x.RightItem.Id).Distinct().ToArray();
            }

            if (skill.IsNullOrEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            foreach (long skillsetId in skill.Distinct())
            {
                // total skilled employees in location
                var skillEmpIds = repositoryEmpSkills.GetBySpecification(new Specification<MToMTableModel<EmployeeModel, SkillsetModel>>(x => x.LeftItem.IsActive == true && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId) && skillsetId == x.RightItem.Id)).Select(x => x.LeftItem.EmpId).ToList();
                int locSkillEmpCount = skillEmpIds.Count();
                if (locSkillEmpCount > 0)
                {
                    var skillProjEmpCount = repositoryEmpProjects.GetBySpecification(new Specification<MToMVersionableTableModel<EmployeeModel, ProjectModel>>(x => x.LeftItem.IsActive == true
                        && x.RightItem.IsActive == true && skillEmpIds.Contains(x.LeftItem.EmpId))).Count();
                    result.ExtendedProperties[skillsetId.ToString()] = locSkillEmpCount.ToString() + "," + skillProjEmpCount.ToString();
                }
                else
                {
                    result.ExtendedProperties[skillsetId.ToString()] = locSkillEmpCount.ToString() + "," + 0.ToString();
                }
            }

            Response<CustomData> results = new Response<CustomData>(result);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        /// <summary>
        /// get employees in a location by skillsets
        /// </summary>
        /// <param name="id">location id</param>
        /// <param name="skill">skills</param>
        /// <param name="flag">billed/unbilled/none</param>
        /// <returns></returns>
        [Route("{id:long:min(1)}/employeebyskills")]
        [HttpGet]
        public HttpResponseMessage GetEmployeeBySkills(long id, [FromUri]long[] skill = null, [FromUri] string flag = "none")
        {
            if (id <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            List<EmployeeLite> result = null;
            result = new List<EmployeeLite>();
            JoinTableRepository<EmployeeModel, LocationModel> repository = new JoinTableRepository<EmployeeModel, LocationModel>();
            IEnumerable<string> employeeIds = repository.GetLeft(id).Select(x => x.EmpId).ToList();
            CommonRepository<MToMTableModel<EmployeeModel, SkillsetModel>> repositoryEmpSkills = new CommonRepository<MToMTableModel<EmployeeModel, SkillsetModel>>();
            CommonRepository<MToMVersionableTableModel<EmployeeModel, ProjectModel>> repositoryEmpProjects = new CommonRepository<MToMVersionableTableModel<EmployeeModel, ProjectModel>>();
            if (skill.IsNullOrEmpty())
            {
                skill = repositoryEmpSkills.GetBySpecification(new Specification<MToMTableModel<EmployeeModel, SkillsetModel>>(x => x.LeftItem.IsActive == true && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId))).Select(x => x.RightItem.Id).Distinct().ToArray();
            }

            if (skill.IsNullOrEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            foreach (long skillsetId in skill.Distinct())
            {
                IEnumerable<EmployeeModel> skillEmps = repositoryEmpSkills.GetBySpecification(new Specification<MToMTableModel<EmployeeModel, SkillsetModel>>(x => x.LeftItem.IsActive == true && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId) && skillsetId == x.RightItem.Id)).Select(x => x.LeftItem).ToList();
                if (!skillEmps.IsNullOrEmpty())
                {
                    switch (flag)
                    {
                        case "billed":
                            employeeIds = skillEmps.Select(x => x.EmpId).ToList();
                            var skillProjEmps = repositoryEmpProjects.GetBySpecification(new Specification<MToMVersionableTableModel<EmployeeModel, ProjectModel>>(x => x.LeftItem.IsActive == true
                                                   && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId))).Select(x => x.LeftItem).ToList();
                            // total skilled employees billable
                            result = ConstructEmployeeLiteList(skillProjEmps);
                            break;
                        case "unbilled":
                            employeeIds = skillEmps.Select(x => x.EmpId).ToList();
                            var skillProjEmpIds = repositoryEmpProjects.GetBySpecification(new Specification<MToMVersionableTableModel<EmployeeModel, ProjectModel>>(x => x.LeftItem.IsActive == true
                                                   && x.RightItem.IsActive == true && employeeIds.Contains(x.LeftItem.EmpId))).Select(x => x.LeftItem.EmpId).ToList();
                            var skillNonProjEmps = skillEmps.Where(x => !skillProjEmpIds.Contains(x.EmpId)).ToList();
                            // total skilled employees non billable
                            result = ConstructEmployeeLiteList(skillNonProjEmps);
                            break;
                        default:
                            // total skilled employees in location
                            result = ConstructEmployeeLiteList(skillEmps);
                            break;
                    }
                }
            }

            if (result.IsNullOrEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<List<EmployeeLite>> results = new Response<List<EmployeeLite>>(result);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }

        private static List<EmployeeLite> ConstructEmployeeLiteList(IEnumerable<EmployeeModel> empList)
        {
            List<EmployeeLite> result = new List<EmployeeLite>();
            foreach (var item in empList)
            {
                if (!result.Any(e => item.EmpId == e.EmpId))
                {
                    result.Add(new EmployeeLite()
                    {
                        Id = item.Id,
                        EmpId = item.EmpId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                    });
                }
            }

            return result;
        }

        // [Route("gis/arm/country/{country}/location")]
        // [HttpGet]
        public HttpResponseMessage GetLocationsByCountry(string country)
        {
            if (string.IsNullOrEmpty(country))
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            CommonService<LocationModel, Location> locationService = new CommonService<LocationModel, Location>();
            IEnumerable<Location> locations = locationService.GetListByFilter(new Specification<LocationModel>(l => l.CountryId == country));
            if (locations.IsNullOrEmpty())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            Response<IEnumerable<Location>> results = new Response<IEnumerable<Location>>(locations);
            return Request.CreateResponse(HttpStatusCode.OK, results);
        }


        private bool TryGetItem(long id, out Location result)
        {
            string cacheKey = "aditi:gis:arm:data:location-" + id.ToString();
            if (!cache.TryGet(cacheKey, out result))
            {
                CommonService<LocationModel, Location> service = new CommonService<LocationModel, Location>();
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

        private bool TryGetAllItems(out IEnumerable<Location> results)
        {
            string cacheKey = "aditi:gis:arm:data:location-all";
            if (!cache.TryGet(cacheKey, out results))
            {
                CommonService<LocationModel, Location> service = new CommonService<LocationModel, Location>();
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
            string cacheKey = "aditi:gis:arm:data:location-" + id.ToString() + "-employee";
            if (!cache.TryGet(cacheKey, out results))
            {
                JoinTableRepository<EmployeeModel, LocationModel> repository = new JoinTableRepository<EmployeeModel, LocationModel>();
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
