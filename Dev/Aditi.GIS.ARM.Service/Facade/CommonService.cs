using System.Collections.Generic;
using Aditi.GIS.ARM.DataAccess.Repository;
using Aditi.GIS.ARM.Shared;
using Aditi.GIS.Foundation.DataAccess;
using Aditi.GIS.Foundation.DataAccess.Entities;
using Aditi.GIS.Foundation.Mapper;
using Aditi.GIS.Foundation.Repository;
using Aditi.GIS.Foundation.Services.Contracts;

namespace Aditi.GIS.ARM.Service.Facade
{
    public class CommonService<TEntity, TContract>
        where TEntity : IEntityModelBase
        where TContract : IContractBase, new()
    {
        private IRepository<TEntity> repository;

        public CommonService()
        {
            this.repository = new CommonRepository<TEntity>();
        }

        public CommonService(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        public TContract Get(long id)
        {
            TEntity entity = this.repository.Get(id);
            TContract target = AutoMapper.Map<TEntity, TContract>(entity);
            return target;
        }

        public IEnumerable<TContract> GetList()
        {
            IEnumerable<TEntity> entityList = this.repository.GetAll();
            IEnumerable<TContract> target = AutoMapper.MapCollection<TEntity, TContract>(entityList);
            return target;
        }

        public IEnumerable<TContract> GetListByFilter(ISpecification<TEntity> specification)
        {
            IEnumerable<TEntity> entityList = this.repository.GetBySpecification(specification);
            IEnumerable<TContract> target = AutoMapper.MapCollection<TEntity, TContract>(entityList);
            return target;
        }
    }
}
