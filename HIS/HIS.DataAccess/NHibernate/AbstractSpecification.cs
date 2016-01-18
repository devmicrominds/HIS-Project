using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace HIS.DataAccess
{
    [Serializable]
    public abstract class AbstractSpecification<T> : ISpecification
    {
		protected AbstractSpecification(){
			this.Initialize();
		}

		protected virtual void Initialize() { 
		
		}
		

        private DetachedCriteria _innerCriteria = null;

        protected virtual DetachedCriteria innerCriteria
        {
            get
            {
                if (_innerCriteria == null)
                {
                    _innerCriteria = DetachedCriteria.For<T>();
                }
                return _innerCriteria;
            }

            set { _innerCriteria = value; }
        }

        public virtual DetachedCriteria GetCriteria()
        {
            return innerCriteria;
        }

		protected virtual DetachedCriteria GetInnerClone () {

			return NHibernate.CriteriaTransformer.Clone (innerCriteria);
		}

        public virtual ISpecification OrderBy(string propertyName, bool ascending)
        {
            if (!String.IsNullOrEmpty(propertyName))
                this.innerCriteria.AddOrder(new Order(propertyName, ascending));

            return this;
			
        }

		public virtual void First () {
			innerCriteria.SetMaxResults (1);
		}
    }

}
