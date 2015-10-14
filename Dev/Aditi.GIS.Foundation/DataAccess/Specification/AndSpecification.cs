namespace Aditi.GIS.Foundation.DataAccess
{
    using System;
    using System.Linq.Expressions;
    using Aditi.GIS.Foundation.DataAccess.Entities;

    /// <summary>
    /// Type for AndAlsoSpecification
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class AndAlsoSpecification<T> : CompositeSpecification<T>
       where T : IEntityModelBase
    {
        #region Members

        private ISpecification<T> rsideSpecification = null;
        private ISpecification<T> lsideSpecification = null;

        #endregion

        #region Public Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="leftSide"></param>
        /// <param name="rightSide"></param>
        public AndAlsoSpecification(ISpecification<T> leftSide, ISpecification<T> rightSide)
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

        #region Composite Specification overrides
        /// <summary>
        /// Left spec
        /// </summary>
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
        /// Propertye for Expression predicate
        /// </summary>
        public override Expression<Func<T, bool>> Predicate
        {
            get
            {
                Expression<Func<T, bool>> left = this.lsideSpecification.Predicate;
                Expression<Func<T, bool>> right = this.rsideSpecification.Predicate;

                return (left.AndAlso(right));
            }
        }

        #endregion
    }
}
