using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;

namespace HIS.DataAccess
{
    public static class NHibernateExtensions
    {
        /// <summary>
        /// Based on the ICriteria will return a paged result set, will create two copies
        /// of the query 1 will be used to select the total count of items, the other
        /// used to select the page of data.
        ///
        /// The results will be wraped in a PagedResult object which will contain
        /// the items and total item count.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="criteria"></param>
        /// <param name="startIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static PagedResult<TEntity> ToPagedResult<TEntity>(this ICriteria criteria, int pageIndex, int pageSize)   {

            // Clone a copy of the criteria, setting a projection
            // to get the row count, this will get the total number of
            // items in the query using a select count(*)
            ICriteria countCriteria = CriteriaTransformer.Clone(criteria)                            
										.SetProjection(Projections.RowCount());
            // Clear the ordering of the results
            countCriteria.ClearOrders();
            // Clone a copy fo the criteria to get the page of data,
            // setting max and first result, this will get the page of data.s
            ICriteria pageCriteria = 
					CriteriaTransformer.Clone(criteria)
                    .SetMaxResults(pageSize)
                    .SetFirstResult(pageIndex);

			//var u = countCriteria.FutureValue<int>();
            // Create a new pagedresult object and populate it, use the paged query
            // to get the items, and the count query to get the total item count.
            var pagedResult = new PagedResult<TEntity>(pageCriteria.List<TEntity>(),
                                                       (int)countCriteria.UniqueResult());
            // Return the result.
            return pagedResult;
        }

        public static PagedResult<TViewModel> ToPagedResult<TEntity, TViewModel>(this ICriteria criteria, int pageIndex, int pageSize) where TViewModel : class {

            // Clone a copy of the criteria, setting a projection
            // to get the row count, this will get the total number of
            // items in the query using a select count(*)
            ICriteria countCriteria = CriteriaTransformer.Clone(criteria)                                        
                                        .SetProjection(Projections.RowCount());
            // Clear the ordering of the results
            countCriteria.ClearOrders();
            // Clone a copy fo the criteria to get the page of data,
            // setting max and first result, this will get the page of data.s
            ICriteria pageCriteria =
                    CriteriaTransformer.Clone(criteria)
                    .SetMaxResults(pageSize)
                    .SetFirstResult(pageIndex);
             
            criteria.SetResultTransformer (Transformers.AliasToBean<TViewModel> ());

            int count = (int)countCriteria.UniqueResult();

            var pagedResult = new PagedResult<TViewModel>();

            //var pagedResult = new PagedResult<TViewModel>(pageCriteria.List<TViewModel>(),
            //                                           (int)countCriteria.UniqueResult());
            // Return the result.
            return pagedResult;
        }

    }
}
