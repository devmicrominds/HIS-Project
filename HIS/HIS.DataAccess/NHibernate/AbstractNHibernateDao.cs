using System;
using System.Collections.Generic;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Criterion;
using System.Collections;
using System.Linq;
using NHibernate.Transform;

namespace HIS.DataAccess
{
    public abstract class AbstractNHibernateDao<T> : AbstractNHibernateDao<T, dynamic>
         where T : class
    {

        public AbstractNHibernateDao(string sessionFactoryConfigPath) : base(sessionFactoryConfigPath) { }
    }

    public abstract class AbstractNHibernateDao<T, IdT> : IDao<T, IdT>
        where T : class
    {
        /// <param name="sessionFactoryConfigPath">Fully qualified path of the session factory's config file</param>
        /// 
        #region Helpers

        protected virtual Order DefaultOrder
        {
            get { return Order.Desc("Id"); }
        }

        protected Order ParseOrder(string sortExpression)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                return DefaultOrder;
            }
            string[] s = sortExpression.Split(' ');
            if (s.Length == 1)
            {
                return new Order(s[0], true);
            }
            else
            {
                return new Order(s[0], s[1] == "ASC");
            }
        }

        #endregion



        public AbstractNHibernateDao(string sessionFactoryConfigPath)
        {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            SessionFactoryConfigPath = sessionFactoryConfigPath;
        }

        /// <summary>
        /// Loads an instance of type T from the DB based on its ID.
        /// </summary>
        public virtual T GetById(IdT id, bool shouldLock)
        {
            T entity;

            if (shouldLock)
            {
                entity = (T)NHibernateSession.Load(persistentType, id, LockMode.Upgrade);
            }
            else
            {
                entity = (T)NHibernateSession.Load(persistentType, id);
            }

            return entity;
        }

        /// <summary>
        /// Loads every instance of the requested type with no filtering.
        /// </summary>
        public virtual List<T> GetAll()
        {
            return GetByCriteria();
        }

        /// <summary>
        /// Loads every instance of the requested type using the supplied <see cref="ICriterion" />.
        /// If no <see cref="ICriterion" /> is supplied, this behaves like <see cref="GetAll" />.
        /// </summary>
        public virtual List<T> GetByCriteria(params ICriterion[] criterion)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persistentType);

            foreach (ICriterion criterium in criterion)
            {
                criteria.Add(criterium);
            }

            return criteria.List<T>() as List<T>;
        }

        public virtual List<T> GetByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persistentType);
            Example example = Example.Create(exampleInstance);

            foreach (string propertyToExclude in propertiesToExclude)
            {
                example.ExcludeProperty(propertyToExclude);
            }

            criteria.Add(example);

            return criteria.List<T>() as List<T>;
        }
        /// <summary>
        /// Looks for a single instance using the example provided.
        /// </summary>
        /// <exception cref="NonUniqueResultException" />
        public virtual T GetUniqueByExample(T exampleInstance, params string[] propertiesToExclude)
        {
            List<T> foundList = GetByExample(exampleInstance, propertiesToExclude);

            if (foundList.Count > 1)
            {
                throw new NonUniqueResultException(foundList.Count);
            }

            if (foundList.Count > 0)
            {
                return foundList[0];
            }
            else
            {
                return default(T);
            }
        }


        public virtual void Flush()
        {

            NHibernateSession.Flush();
        }

        /// <summary>
        /// For entities that have assigned ID's, you must explicitly call Save to add a new one.
        /// See http://www.hibernate.org/hib_docs/reference/en/html/mapping.html#mapping-declaration-id-assigned.
        /// </summary>
        public virtual T Save(T entity)
        {
            NHibernateSession.Save(entity);
            return entity;
        }

        public T Merge(T entity)
        {

            T mergedEntity = NHibernateSession.Merge(entity);
            return mergedEntity;
        }



        public T Persist(T entity)
        {

            NHibernateSession.Persist(entity);
            return entity;
        }

        public void Evict(T entity)
        {

            NHibernateSession.Evict(entity);
        }

        public void Lock(T entity)
        {

            NHibernateSession.Lock(entity, LockMode.None);
        }

        public void Update(T entity)
        {

            NHibernateSession.Update(entity);
        }


        public void CloseSession()
        {

            NHibernateSessionManager.Instance.CloseSessionOn(this.SessionFactoryConfigPath);
        }

        /// <summary>
        /// For entities with automatically generated IDs, such as identity, SaveOrUpdate may 
        /// be called when saving a new entity.  SaveOrUpdate can also be called to _update_ any 
        /// entity, even if its ID is assigned.
        /// </summary>
        public virtual T SaveOrUpdate(T entity)
        {
            NHibernateSession.SaveOrUpdate(entity);

            return entity;
        }

        public virtual void Delete(T entity)
        {
            NHibernateSession.Delete(entity);
        }

        /// <summary>
        /// Commits changes regardless of whether there's an open transaction or not
        /// </summary>
        public void CommitChanges()
        {
            if (NHibernateSessionManager.Instance.HasOpenTransactionOn(SessionFactoryConfigPath))
            {
                NHibernateSessionManager.Instance.CommitTransactionOn(SessionFactoryConfigPath);
            }
            else
            {
                // If there's no transaction, just flush the changes
                NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath).Flush();
            }
        }



        public void CreateNewSchema()
        {

            NHibernateSessionManager.Instance.CreateNewSchemaFor(SessionFactoryConfigPath);
        }

        /// <summary>
        /// Exposes the ISession used within the DAO.
        /// </summary>
        protected ISession NHibernateSession
        {
            get
            {
                return NHibernateSessionManager.Instance.GetSessionFrom(SessionFactoryConfigPath);
            }
        }

        private Type persistentType = typeof(T);
        protected readonly string SessionFactoryConfigPath;

        protected virtual IList<T> FindAll(DetachedCriteria detachedCriteria)
        {
            //            using (ISession session = NHibernateSession.GetSession(EntityMode.Poco))
            ISession session = NHibernateSession.GetSession(EntityMode.Poco);
            {
                ICriteria criteria = detachedCriteria.GetExecutableCriteria(session);

                return criteria.List<T>();
            }
        }

        public virtual IList<T> FindAll(ISpecification spec)
        {
            var detachedCriteria = GetDetachedCriteriaFromSpecification(spec);
            return FindAll(detachedCriteria);
        }

        /// <summary>
        /// use for group by type , projections
        /// that does not require strong type
        /// </summary>
        /// <param name="detachedCriteria"></param>
        /// <returns></returns>
        public virtual IList ListAll(DetachedCriteria detachedCriteria)
        {

            ISession session = NHibernateSession.GetSession(EntityMode.Poco);
            {
                ICriteria criteria = detachedCriteria.GetExecutableCriteria(session);

                return criteria.List();
            }

        }

        public virtual IList ListAll(ICriteria criteria)
        {

            return criteria.List();
        }


        public virtual IList ListAll(ISpecification spec)
        {

            var detachedCriteria = GetDetachedCriteriaFromSpecification(spec);
            return ListAll(detachedCriteria);
        }

        public virtual IList<TViewModel> ListAll<TViewModel>(ICriteria criteria)
        {

            return criteria.SetResultTransformer(Transformers.AliasToBean<TViewModel>()).List<TViewModel>();
        }

        public virtual IList<TViewModel> ListAll<TViewModel>(DetachedCriteria detachedCriteria)
        {


            ISession session = NHibernateSession.GetSession(EntityMode.Poco);
            {
                ICriteria criteria = detachedCriteria.GetExecutableCriteria(session);

                return criteria.SetResultTransformer(Transformers.AliasToBean<TViewModel>())
                    .List<TViewModel>();
            }

        }

        protected virtual TResult Transact<TResult>(Func<TResult> func)
        {

            if (!NHibernateSession.Transaction.IsActive)
            {
                using (var tx = NHibernateSession.BeginTransaction())
                {

                    TResult result;

                    result = func.Invoke();
                    tx.Commit();
                    return result;



                }
            }

            return func.Invoke();
        }

        public virtual void Transact(Action action)
        {

            Transact<bool>(() =>
            {
                action.Invoke();
                return false;
            });
        }



        /// <summary>
        /// Check specification object is valid and contains DetachedCriteria
        /// Will throw exception if invalid.
        /// </summary>
        /// <param name="spec"></param>
        private DetachedCriteria GetDetachedCriteriaFromSpecification(ISpecification spec)
        {
            if (spec == null)
                throw new ArgumentNullException("spec");

            var detachedCriteria = spec.GetCriteria();
            if (detachedCriteria == null)
                throw new ArgumentException("Specification passed contains null criteria object", "spec");

            return detachedCriteria;
        }

        public virtual PagedResult<T> FindAll(ISpecification spec, int pageIndex, int pageSize)
        {
            //using (ISession session = NHibernateSession.GetSession(EntityMode.Poco))
            ISession session = NHibernateSession.GetSession(EntityMode.Poco);
            {
                var detachedCriteria = GetDetachedCriteriaFromSpecification(spec);
                ICriteria criteria = detachedCriteria.GetExecutableCriteria(session);
                PagedResult<T> pagedResult = criteria.ToPagedResult<T>(pageIndex, pageSize);
                return pagedResult;

            }
        }



        public virtual PagedResult<T> FindAll(DetachedCriteria dc, int pageIndex, int pageSize)
        {

            ISession session = NHibernateSession.GetSession(EntityMode.Poco);
            ICriteria criteria = dc.GetExecutableCriteria(session);
            PagedResult<T> pagedResult = criteria.ToPagedResult<T>(pageIndex, pageSize);
            return pagedResult;
        }



        public virtual void DeleteAll()
        {
            ISession session = NHibernateSession.GetSession(EntityMode.Poco);
            session.Delete("from " + typeof(T).Name);
            session.Flush();
        }

        public PagedResult<T> FindAll(int pageIndex, int pageSize)
        {
            ICriteria criteria = NHibernateSession.CreateCriteria(persistentType);
            return criteria.ToPagedResult<T>(pageIndex, pageSize);
        }

        public virtual T Single(ISpecification spec)
        {
            var detachedCriteria = GetDetachedCriteriaFromSpecification(spec);
            ICriteria criteria = detachedCriteria.GetExecutableCriteria(NHibernateSession.GetSession(EntityMode.Poco));

            return criteria.SetMaxResults(1).UniqueResult<T>();
        }

        public virtual int GetRowCount(ISpecification spec)
        {

            var detachedCriteria = GetDetachedCriteriaFromSpecification(spec);
            ICriteria criteria = detachedCriteria.GetExecutableCriteria(NHibernateSession.GetSession(EntityMode.Poco));
            criteria.SetProjection(Projections.RowCount());
            return (int)criteria.UniqueResult();

        }


        public IMultiCriteria GetMultiCriteria()
        {

            return NHibernateSession.CreateMultiCriteria();
        }

        public IQueryable<T> GetQueryable()
        {

            var query = NHibernateSession.Query<T>();
            query = query.Cacheable<T>();

            return query;

        }

        public IQueryable<T> GetQueryableRead()
        {
            NHibernateSession.FlushMode = FlushMode.Never;
            var query = NHibernateSession.Query<T>();
            query = query.Cacheable<T>();
            return query;
        }

        public IQueryable<T> GetQueryableNonCache()
        {

            var query = NHibernateSession.Query<T>();

            return query;
        }

        public IQueryOver<T, T> GetQueryOver()
        {

            return NHibernateSession.QueryOver<T>();
        }

        public ICriteria GetCriteria()
        {

            return NHibernateSession.CreateCriteria<T>("p");
        }

        public PagedResult<T> PagedResult(IQueryable<T> query, int pageIndex, int pageSize)
        {

            var rowCount = query.Count();
            var result = query.Skip(pageIndex * pageSize)
                                .Take(pageSize)
                                .ToList();

            return new PagedResult<T>(result, rowCount);
        }


    }
}
