namespace Aditi.GIS.Foundation.DataAccess
{
    using System;
    using System.Linq.Expressions;
    using Aditi.GIS.Foundation.DataAccess.Entities;

    /// <summary>
    /// Type for Specification
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class Specification<TEntity>
         : ISpecification<TEntity>
         where TEntity : IEntityModelBase
    {
        /// <summary>
        /// Delegate for predicate
        /// </summary>
        protected Func<TEntity, bool> predicateCompiled;
        /// <summary>
        /// Predicate Expression
        /// </summary>
        public virtual Expression<Func<TEntity, bool>> Predicate { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Specification()
        {

        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="predicate"></param>
        public Specification(Expression<Func<TEntity, bool>> predicate)
        {
            this.Predicate = predicate;
            this.predicateCompiled = predicate.Compile();
        }

        /// <summary>
        /// Method to check satisfied by condition
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool SatisfiedBy(TEntity entity)
        {
            if (predicateCompiled != null)
            {
                return predicateCompiled.Invoke(entity);
            }
            else
            {
                throw new InvalidOperationException("Predicate");
            }
        }

        /// <summary>
        /// Method to add And spec
        /// </summary>
        /// <param name="rightSideSpecification"></param>
        /// <returns></returns>
        public Specification<TEntity> AndAlso(ISpecification<TEntity> rightSideSpecification)
        {
            return new AndAlsoSpecification<TEntity>(this, rightSideSpecification);

        }

        /// <summary>
        /// Method to add Or spec
        /// </summary>
        /// <param name="rightSideSpecification"></param>
        /// <returns></returns>
        public Specification<TEntity> OrElse(ISpecification<TEntity> rightSideSpecification)
        {
            return new AndAlsoSpecification<TEntity>(this, rightSideSpecification);

        }


        #region Override Operators
        /// <summary>
        /// Operator overload for And spec
        /// </summary>
        /// <param name="leftSideSpecification"></param>
        /// <param name="rightSideSpecification"></param>
        /// <returns></returns>
        public static Specification<TEntity> operator &(Specification<TEntity> leftSideSpecification, ISpecification<TEntity> rightSideSpecification)
        {
            return new AndAlsoSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Operator overload for Or spec
        /// </summary>
        /// <param name="leftSideSpecification"></param>
        /// <param name="rightSideSpecification"></param>
        /// <returns></returns>
        public static Specification<TEntity> operator |(Specification<TEntity> leftSideSpecification, ISpecification<TEntity> rightSideSpecification)
        {
            return new OrElseSpecification<TEntity>(leftSideSpecification, rightSideSpecification);
        }

        /// <summary>
        /// Operator overload for Not spec
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static Specification<TEntity> operator !(Specification<TEntity> specification)
        {
            return new NotSpecification<TEntity>(specification);
        }

        /// <summary>
        /// Operator overload for false spec
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static bool operator false(Specification<TEntity> specification)
        {
            return false;
        }

        /// <summary>
        /// operator overload for true spec
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        public static bool operator true(Specification<TEntity> specification)
        {
            return false;
        }

        #endregion
    }
}

