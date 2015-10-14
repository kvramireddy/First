namespace Aditi.GIS.Foundation.DataAccess
{
    using System;
    using System.Linq.Expressions;
    using Aditi.GIS.Foundation.DataAccess.Entities;

    /// <summary>
    /// Interface for Specification
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface ISpecification<TEntity>
        where TEntity : IEntityModelBase
    {
        /// <summary>
        /// Method for getting the Expression
        /// </summary>
        Expression<Func<TEntity, bool>> Predicate { get; }
        /// <summary>
        /// Method to check Specifation satisfaction
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool SatisfiedBy(TEntity entity);
    }
}
