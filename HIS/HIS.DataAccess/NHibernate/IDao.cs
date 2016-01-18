using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HIS.DataAccess
{
    public interface IDao<T, IdT>
    {
        T GetById(IdT id, bool shouldLock);
        List<T> GetAll();
        List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude);
        T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude);

		void Evict (T entity);
		T Persist (T entity);
		T Merge (T entity);
        T Save(T entity);
        T SaveOrUpdate(T entity);
        void Delete(T entity);
        void CommitChanges();
		
		
        IList<T> FindAll(ISpecification spec);
		IList ListAll (ISpecification spec);
		T Single(ISpecification spec);

        PagedResult<T> FindAll(ISpecification spec, int pageIndex, int pageSize);
        PagedResult<T> FindAll(int pageIndex, int pageSize);
        PagedResult<T> PagedResult(IQueryable<T> query, int pageIndex, int pageSize);

        void DeleteAll();

		IQueryable<T> GetQueryable ();
		IQueryOver<T,T> GetQueryOver ();
		IMultiCriteria GetMultiCriteria ();
		ICriteria GetCriteria ();

        
		void Transact (Action action);
		
    }
}
