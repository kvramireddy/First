namespace Aditi.GIS.Foundation.DataAccess
{
    using Aditi.GIS.Foundation.DataAccess.Entities;

    /// <summary>
    /// Composition specification
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    public abstract class CompositeSpecification<TEntity> : Specification<TEntity>
        where TEntity : IEntityModelBase
    {
        /// <summary>
        /// Gets the left side specification.
        /// </summary>
        /// <value>
        /// The left side specification.
        /// </value>
        public abstract ISpecification<TEntity> LeftSideSpecification { get; }
        /// <summary>
        /// Gets the right side specification.
        /// </summary>
        /// <value>
        /// The right side specification.
        /// </value>
        public abstract ISpecification<TEntity> RightSideSpecification { get; }

        /// <summary>
        /// Satisfieds the by.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public override bool SatisfiedBy(TEntity entity)
        {
            if (this.predicateCompiled == null)
            {
                this.predicateCompiled = this.Predicate.Compile();
            }
            return base.SatisfiedBy(entity);
        }
    }
}
