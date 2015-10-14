namespace Aditi.GIS.Foundation.DataAccess
{
    using System;
    using System.Linq.Expressions;
    using Aditi.GIS.Foundation.DataAccess.Entities;
    /// <summary>
    /// Type for OrElseSpecification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class OrElseSpecification<T> : CompositeSpecification<T>
         where T : IEntityModelBase
    {
        #region Members

        private ISpecification<T> rsideSpecification = null;
        private ISpecification<T> lsideSpecification = null;

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        #region Public Constructor
        public OrElseSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
        {
            if (leftSide == (ISpecification<T>)null)
            {
                throw new ArgumentNullException("leftSide");
            }

            if (rightSide == (ISpecification<T>)null)
            {
                throw new ArgumentNullException("rightSide");
            }

            this.lsideSpecification = leftSide;
            this.rsideSpecification = rightSide;
        }

        #endregion

        /// <summary>
        /// Left spec
        /// </summary>
        #region Composite Specification overrides
        public override ISpecification<T> LeftSideSpecification
        {
            get { return this.lsideSpecification; }
        }

        /// <summary>
        /// Right spec
        /// </summary>
        public override ISpecification<T> RightSideSpecification
        {
            get { return this.rsideSpecification; }
        }

        /// <summary>
        /// Predicate Expression
        /// </summary>
        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                Expression<Func<T, bool>> left = this.lsideSpecification.Predicate;
                Expression<Func<T, bool>> right = this.rsideSpecification.Predicate;

                return (left.OrElse(right));
            }
        }

        #endregion
    }
}
