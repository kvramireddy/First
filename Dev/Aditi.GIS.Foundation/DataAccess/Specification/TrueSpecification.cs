namespace Aditi.GIS.Foundation.DataAccess
{
    using System;
    using System.Linq.Expressions;
    using Aditi.GIS.Foundation.DataAccess.Entities;
    /// <summary>
    /// Type for TrueSpecification
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public sealed class TrueSpecification<TEntity> :Specification<TEntity>
        where TEntity : IEntityModelBase
    {
        #region Specification overrides
        /// <summary>
        /// Property to get Predicate
        /// </summary>
        public override System.Linq.Expressions.Expression<Func<TEntity, bool>> Predicate
        {
            get
            {
                bool result = true;

                Expression<Func<TEntity, bool>> trueExpression = t => result;
                return trueExpression;
            }
        }

        #endregion
    }
}
