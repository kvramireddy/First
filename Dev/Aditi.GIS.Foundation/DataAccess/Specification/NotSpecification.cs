namespace Aditi.GIS.Foundation.DataAccess
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Aditi.GIS.Foundation.DataAccess.Entities;
    /// <summary>
    /// Type for NotSpecification
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class NotSpecification<TEntity>
        : Specification<TEntity>
        where TEntity : IEntityModelBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actualSpecification"></param>
        public NotSpecification(ISpecification<TEntity> actualSpecification)
            : base(actualSpecification.Predicate)
        {

        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="actualPredicate"></param>
        public NotSpecification(Expression<Func<TEntity, bool>> actualPredicate)
            : base(actualPredicate)
        {
        }
        /// <summary>
        /// Property for getting the Expression predicate
        /// </summary>
        public override Expression<Func<TEntity, bool>> Predicate
        {
            get
            {
                return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(this.Predicate.Body),
                                                       this.Predicate.Parameters.Single());
            }
        }
    }
}
