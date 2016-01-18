using System;
using System.Collections;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Web;
using NHibernate;
using NHibernate.Cache;
using NHibernate.Cfg;   
using NHibernate.Mapping.ByCode;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Tool.hbm2ddl;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Event;
using NHibernate.Caches.SysCache;
using HIS.DataAccess.Session;
using System.Data;
                                     

namespace HIS.DataAccess
{
    /// <summary>
    /// Handles creation and management of sessions and transactions.  It is a singleton because 
    /// building the initial session factory is very expensive. Inspiration for this class came 
    /// from Chapter 8 of Hibernate in Action by Bauer and King.  Although it is a sealed singleton
    /// you can use TypeMock (http://www.typemock.com) for more flexible testing.
    /// </summary>
    public sealed class NHibernateSessionManager : ISessionFactoryProvider {

		static Dictionary<string, string> mappingAssembly;
        static Dictionary<string, string> domainAssembly;
        static Dictionary<string, Configuration> configurations; 

        #region Thread-safe, lazy Singleton

        /// <summary>
        /// This is a thread-safe, lazy singleton.  See http://www.yoda.arachsys.com/csharp/singleton.html
        /// for more details about its implementation.
        /// </summary>
		
        public static NHibernateSessionManager Instance
        {
            get
            {
                return Nested.NHibernateSessionManager;
            }
        }

        /// <summary>
        /// Private constructor to enforce singleton
        /// </summary>
        private NHibernateSessionManager() { }

        /// <summary>
        /// Assists with ensuring thread-safe, lazy singleton
        /// </summary>
        private class Nested
        {
            static Nested() { }			
			internal static readonly NHibernateSessionManager NHibernateSessionManager =
                new NHibernateSessionManager();
        }

        #endregion

        /// <summary>
        /// This method attempts to find a session factory stored in <see cref="sessionFactories" />
        /// via its name; if it can't be found it creates a new one and adds it the hashtable.
        /// </summary>
        /// <param name="sessionFactoryConfigPath">Path location of the factory config</param>
        public ISessionFactory GetSessionFactoryFor(string sessionFactoryConfigPath)
        {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  Attempt to retrieve a stored SessionFactory from the hashtable.
          ISessionFactory sessionFactory = (ISessionFactory)sessionFactories[sessionFactoryConfigPath];
			
            //  Failed to find a matching SessionFactory so make a new one.
            if (sessionFactory == null)
            {
                Check.Require(File.Exists(sessionFactoryConfigPath),
                    "The config file at '" + sessionFactoryConfigPath + "' could not be found");

                Configuration cfg = new Configuration();
				
				
                cfg.Configure(sessionFactoryConfigPath);

                var map = GetMappings(sessionFactoryConfigPath);
                cfg.AddDeserializedMapping(map, "NHSchemaTest");

				//InitializeEventListeners (cfg);
                cfg.Cache(u =>
                {
                    u.UseQueryCache = true;
                    u.RegionsPrefix = "LongTerm";

                }).SessionFactory().Caching.Through<SysCacheProvider>();
                //  Now that we have our Configuration object, create a new SessionFactory
                sessionFactory = cfg.BuildSessionFactory();

                
                if (sessionFactory == null)
                {
                    throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
                }

                if (null == configurations)
                    configurations = new Dictionary<string, Configuration>();

                configurations[sessionFactoryConfigPath] = cfg;
                sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
                //HttpRuntime.Cache.Add(sessionFactoryConfigPath,sessionFactory, null, DateTime.Now.AddDays(7),
                //   TimeSpan.Zero, CacheItemPriority.High, null);

                
            }

            return sessionFactory;
        }

		private void InitializeEventListeners (Configuration cfg) {

			if ( null != postInsertEventListeners ) 
				cfg.EventListeners.PostInsertEventListeners = postInsertEventListeners;
			

			if ( null != preInsertEventListeners ) 
				cfg.EventListeners.PreInsertEventListeners = preInsertEventListeners;
			

			if ( null != preUpdateEventListeners )
				cfg.EventListeners.PreUpdateEventListeners = preUpdateEventListeners;
			

			if ( null != postUpdateEventListeners )
				cfg.EventListeners.PostUpdateEventListeners = postUpdateEventListeners;
			
		
		}

		private void InitializeFilters (Configuration cfg) {

			var filterDef = new FilterDefinition (
				"contextFilter",
				null, // or your default condition
				new Dictionary<string, IType> { { "current", NHibernateUtil.Int32 } },
				false);

			cfg.AddFilterDefinition (filterDef);
		
		
		}

        public void CreateNewSchemaFor(string sessionFactoryConfigPath)
        {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  Attempt to retrieve a stored SessionFactory from the hashtable.
            if (sessionFactories.ContainsKey(sessionFactoryConfigPath))
                sessionFactories.Remove(sessionFactoryConfigPath);

             
            //if (null != HttpRuntime.Cache[sessionFactoryConfigPath])
            //    HttpRuntime.Cache.Remove(sessionFactoryConfigPath);

            Check.Require(File.Exists(sessionFactoryConfigPath),
                "The config file at '" + sessionFactoryConfigPath + "' could not be found");

            Configuration cfg = new Configuration();
			InitializeFilters (cfg);
            cfg.Configure(sessionFactoryConfigPath);
            var map = GetMappings(sessionFactoryConfigPath);
            cfg.AddDeserializedMapping(map, "NHSchemaTest");
            cfg.CreateIndexesForForeignKeys();

            //new SchemaExport(cfg).Create(true, true);
            //new SchemaExport(cfg).Drop(true, true);
            new SchemaExport(cfg).Create(true, true);

            //  Now that we have our Configuration object, create a new SessionFactory
            ISessionFactory sessionFactory = cfg.BuildSessionFactory();

            if (sessionFactory == null)
            {
                throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
            }

            //HttpRuntime.Cache.Add(sessionFactoryConfigPath, sessionFactory, null, DateTime.Now.AddDays(7),
            //      TimeSpan.Zero, CacheItemPriority.High, null);

            sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
        }

        public void DropSchema(string sessionFactoryConfigPath)
        {
            Configuration cfg = new Configuration();
			InitializeFilters (cfg);
            cfg.Configure(sessionFactoryConfigPath);

            new SchemaExport(cfg).Drop(true, true);
        }

		public void UpdateSchemaFor (string sessionFactoryConfigPath) {

			Configuration cfg = new Configuration ();
			InitializeFilters (cfg);
			cfg.Configure (sessionFactoryConfigPath);

			var map = GetMappings (sessionFactoryConfigPath);
			cfg.AddDeserializedMapping (map, "NHSchemaTest");  
			var update = new SchemaUpdate (cfg);
			update.Execute (true,true);
			
		}

        private HbmMapping GetMappings(string sessionFactoryConfigPath) {

            if (domainAssembly.ContainsKey(sessionFactoryConfigPath))
            {
                var domm = domainAssembly[sessionFactoryConfigPath];
                var assm = mappingAssembly[sessionFactoryConfigPath];

                ModelMapper mapper = new ModelMapper();
                var entities = new List<Type>();
                var dataEntities = Assembly.Load(domm).GetTypes();
                var mapping = new List<Type>();
                var entityMapping = Assembly.Load(assm).GetTypes().Where(o=>o.Name.EndsWith("Map"));

                foreach (var e in dataEntities)
                    entities.Add(e);

                foreach (var m in entityMapping)
                    mapper.AddMapping(m);

                HbmMapping domainMapping = mapper.CompileMappingFor(entities);

                return domainMapping;

            }
            else
            {
                var assm = mappingAssembly[sessionFactoryConfigPath];


                ModelMapper mapper = new ModelMapper();
                var entities = new List<Type>();
                var dataEntities = Assembly.Load(assm).GetTypes();
                foreach (var e in dataEntities)
                {
                    if (e.Name.EndsWith("Map"))
                    {
                        //System.Diagnostics.Debug.WriteLine (e.Name);
                        mapper.AddMapping(e);
                    }
                    else
                    {
                        entities.Add(e);
                        //System.Diagnostics.Debug.WriteLine (e.Name);
                    }
                }

                HbmMapping domainMapping = mapper.CompileMappingFor(entities);

                return domainMapping;
            }
        }

        /// <summary>
        /// Allows you to register an interceptor on a new session.  This may not be called if there is already
        /// an open session attached to the HttpContext.  If you have an interceptor to be used, modify
        /// the HttpModule to call this before calling BeginTransaction().
        /// </summary>
        public void RegisterInterceptorOn(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen)
            {
                throw new CacheException("You cannot register an interceptor once a session has already been opened");
            }

            GetSessionFrom(sessionFactoryConfigPath, interceptor);
        }

        public ISession GetSessionFrom(string sessionFactoryConfigPath)  {
			
            return GetSessionFrom(sessionFactoryConfigPath, null);
        }

        public IStatelessSession GetStatelessSessionFrom(string sessionFactoryConfigPath) {

            return GetSessionFactoryFor(sessionFactoryConfigPath).OpenStatelessSession();
        }

        /// <summary>
        /// Gets a session with or without an interceptor.  This method is not called directly; instead,
        /// it gets invoked from other public methods.
        /// </summary>
        private ISession GetSessionFrom(string sessionFactoryConfigPath, IInterceptor interceptor)
        {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session == null)
            {
                if (interceptor != null)
                {
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession(interceptor);
                }
                else
                {
                    
                    //System.Diagnostics.Debug.WriteLine("Get New Session");
                    session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
                }

                ContextSessions[sessionFactoryConfigPath] = session;
            }

            if (!session.IsOpen) {
                session = GetSessionFactoryFor(sessionFactoryConfigPath).OpenSession();
            }

            Check.Ensure(session != null, "session was null");

            return session;
        }

        /// <summary>
        /// Flushes anything left in the session and closes the connection.
        /// </summary>
        public void CloseSessionOn(string sessionFactoryConfigPath)
        {
            ISession session = (ISession)ContextSessions[sessionFactoryConfigPath];

            if (session != null && session.IsOpen)  {

                //session.Close();
                session.Dispose();
				
            }

            if (ContextSessions.ContainsKey(sessionFactoryConfigPath)){

                //System.Diagnostics.Debug.WriteLine("Remove Session");
                ContextSessions.Remove(sessionFactoryConfigPath);
            }
        }


		public void Flush (string sessionFactoryConfigPath) {

			ISession session = (ISession) ContextSessions [sessionFactoryConfigPath];

			if ( session != null && session.IsOpen ) {

				session.Flush ();				
			}
		}
		
        public ITransaction BeginTransactionOn(string sessionFactoryConfigPath)
        {
			ITransaction transaction = (ITransaction) ContextTransactions [sessionFactoryConfigPath];
			if ( null == transaction ) {
				transaction = GetSessionFrom (sessionFactoryConfigPath).BeginTransaction ();
				ContextTransactions.Add (sessionFactoryConfigPath, transaction);
			}
            

            return transaction;
        }
       
        public void ClearTransactionOn(string sessionFactoryConfigPath) {
 
            ITransaction transaction = (ITransaction) ContextTransactions [sessionFactoryConfigPath];
            if (null != transaction) {
                ContextTransactions.Remove(sessionFactoryConfigPath);
            }
        
        }

        public void CommitTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

			try {

				if ( HasOpenTransactionOn (sessionFactoryConfigPath) ) {

                    //System.Diagnostics.Debug.WriteLine("Commits Start");
                    Flush(sessionFactoryConfigPath);
					transaction.Commit ();
					ContextTransactions.Remove (sessionFactoryConfigPath);
                    //System.Diagnostics.Debug.WriteLine("Commit End");
				}
			}
			catch ( HibernateException ex ) {

                //System.Diagnostics.Debug.WriteLine("Commit Error");
				RollbackTransactionOn (sessionFactoryConfigPath);
				throw;
			}
			
			
        }

        public bool HasOpenTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            return transaction != null && !transaction.WasCommitted && !transaction.WasRolledBack;
        }

        public void RollbackTransactionOn(string sessionFactoryConfigPath)
        {
            ITransaction transaction = (ITransaction)ContextTransactions[sessionFactoryConfigPath];

            try
            {
                if (HasOpenTransactionOn(sessionFactoryConfigPath))
                {
                    transaction.Rollback();
                }

                //if(ContextTransactions.ContainsKey(sessionFactoryConfigPath))
                //    ContextTransactions.Remove(sessionFactoryConfigPath);

            }
            finally
            {
                CloseSessionOn(sessionFactoryConfigPath);
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one transaction per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
        private Hashtable ContextTransactions
        {
            get
            {
                if (IsInWebContext())
                {
                    if (HttpContext.Current.Items[TRANSACTION_KEY] == null)
                        HttpContext.Current.Items[TRANSACTION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[TRANSACTION_KEY];
                }
                else
                {
                    if (CallContext.GetData(TRANSACTION_KEY) == null)
                        CallContext.SetData(TRANSACTION_KEY, new Hashtable());

                    return (Hashtable)CallContext.GetData(TRANSACTION_KEY);
                }
            }
        }

        /// <summary>
        /// Since multiple databases may be in use, there may be one session per database 
        /// persisted at any one time.  The easiest way to store them is via a hashtable
        /// with the key being tied to session factory.  If within a web context, this uses
        /// <see cref="HttpContext" /> instead of the WinForms specific <see cref="CallContext" />.  
        /// Discussion concerning this found at http://forum.springframework.net/showthread.php?t=572
        /// </summary>
				
        private Hashtable ContextSessions
        {
            get
            {
                if (IsInWebContext())
                {
                    if (HttpContext.Current.Items[SESSION_KEY] == null)
                        HttpContext.Current.Items[SESSION_KEY] = new Hashtable();

                    return (Hashtable)HttpContext.Current.Items[SESSION_KEY];
                }
                else
                {
                    if (CallContext.GetData(SESSION_KEY) == null)
                        CallContext.SetData(SESSION_KEY, new Hashtable());

                    return (Hashtable) CallContext.GetData(SESSION_KEY);
                }
            }
        }

        private bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }

        internal static ISession WebSession
        {
            get
            {
                Hashtable o = (Hashtable)HttpContext.Current.Items[SESSION_KEY];
                ISession result = null;
                foreach (var val in o.Values)   {
                    result = (ISession)val;

                }
                return result;

            }
            set
            {
                HttpContext.Current.Items[SESSION_KEY] = value;
            }

        }

        private Hashtable sessionFactories = new Hashtable();
        private const string TRANSACTION_KEY = "CONTEXT_TRANSACTIONS";
        private const string SESSION_KEY = "CONTEXT_SESSIONS";

        public void UpdateSchema(string sessionFactoryConfigPath)
        {
            Check.Require(!string.IsNullOrEmpty(sessionFactoryConfigPath),
                "sessionFactoryConfigPath may not be null nor empty");

            //  Attempt to retrieve a stored SessionFactory from the hashtable.
            if (sessionFactories.ContainsKey(sessionFactoryConfigPath))
                sessionFactories.Remove(sessionFactoryConfigPath);

            //if (null != HttpRuntime.Cache[sessionFactoryConfigPath])
            //    HttpRuntime.Cache.Remove(sessionFactoryConfigPath);


            Check.Require(File.Exists(sessionFactoryConfigPath),
                "The config file at '" + sessionFactoryConfigPath + "' could not be found");

            Configuration cfg = new Configuration();
			InitializeFilters (cfg);
            cfg.Configure(sessionFactoryConfigPath);
            
            var map = GetMappings(sessionFactoryConfigPath);
            cfg.AddDeserializedMapping(map, "NHSchemaTest");
            cfg.CreateIndexesForForeignKeys();

            //new SchemaExport(cfg).Drop(true, true);
            //new SchemaExport(cfg).Create(true, true);

            //  Now that we have our Configuration object, create a new SessionFactory

            var update = new SchemaUpdate(cfg);
            update.Execute(true, true);

            ISessionFactory sessionFactory = cfg.BuildSessionFactory();

            if (sessionFactory == null)
            {
                throw new InvalidOperationException("cfg.BuildSessionFactory() returned null.");
            }

            sessionFactories.Add(sessionFactoryConfigPath, sessionFactory);
            //HttpRuntime.Cache.Add(sessionFactoryConfigPath, sessionFactory, null, DateTime.Now.AddDays(7),
            //   TimeSpan.Zero, CacheItemPriority.High, null);

        }

        


        #region EventListener

        public void SetPostInsertEventListeners (IEnumerable<IPostInsertEventListener> postInsertEventListeners) {
			
			this.postInsertEventListeners = postInsertEventListeners.ToArray();
		}

		public void SetPreInsertEventListeners (IEnumerable<IPreInsertEventListener> preInsertEventListeners) {

			this.preInsertEventListeners = preInsertEventListeners.ToArray ();
		}

		public void SetPreUpdateEventListeners (IEnumerable<IPreUpdateEventListener> preUpdateEventListeners) {

			this.preUpdateEventListeners = preUpdateEventListeners.ToArray();
		}

		public void SetPostUpdateEventListeners (IEnumerable<IPostUpdateEventListener> postUpdateEventListeners) {

			this.postUpdateEventListeners = postUpdateEventListeners.ToArray ();
		}

		private IPostInsertEventListener [] postInsertEventListeners;
		private IPreInsertEventListener [] preInsertEventListeners;		
		private IPostUpdateEventListener [] postUpdateEventListeners;
		private IPreUpdateEventListener [] preUpdateEventListeners;

        #endregion

        public void AddMappingAssembly (string sessionFactoryConfigPath, string mappingAssemblyName) {

			if ( null == mappingAssembly )
				mappingAssembly = new Dictionary<string, string> ();
			
			mappingAssembly [sessionFactoryConfigPath] = mappingAssemblyName;
		}

        public void AddMappingAssembly(string sessionFactoryConfigPath, string domainAssemblyName, string mappingAssemblyName)
        {

            if (null == domainAssembly)
                domainAssembly = new Dictionary<string, string>();

            if (null == mappingAssembly)
                mappingAssembly = new Dictionary<string, string>();

            domainAssembly[sessionFactoryConfigPath] = domainAssemblyName;
            mappingAssembly[sessionFactoryConfigPath] = mappingAssemblyName; 
        }


        public IEnumerable<ISessionFactory> GetSessionFactories()
        {
            return this.sessionFactories.Values.OfType<ISessionFactory>();
        }

        public string GetDbConnection(string sessionFactoryConfigPath)
        {
            return String.Empty;
        }

        public Configuration GetConfiguration(string sessionFactoryConfigPath)
        {
            return configurations[sessionFactoryConfigPath];
        }
    }
}
