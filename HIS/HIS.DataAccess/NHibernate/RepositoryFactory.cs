using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.DataAccess
{
    public class RepositoryFactory : IRepositoryFactory
    {
         
        private string sessionFactoryConfigPath;
        public RepositoryFactory(string sessionFactoryConfigPath)
            : this(sessionFactoryConfigPath, "HIS.Data")
        {
        }

        public RepositoryFactory(string sessionFactoryConfigPath, string domainAssembly)
        {
            Check.Require(sessionFactoryConfigPath != null, "sessionFactoryConfigPath may not be null");
            SessionFactoryConfigPath = sessionFactoryConfigPath;

            NHibernateSessionManager.Instance.AddMappingAssembly(sessionFactoryConfigPath, domainAssembly);

        }

        public RepositoryFactory(string sessionFactoryConfigPath, string domainAssembly, string mappingAssembly)
        {
            Check.Require(sessionFactoryConfigPath != null, "sessionFactoryConfigPath may not be null");
            SessionFactoryConfigPath = sessionFactoryConfigPath;
            NHibernateSessionManager.Instance.AddMappingAssembly(sessionFactoryConfigPath, domainAssembly,mappingAssembly);
        } 
       
        public string SessionFactoryConfigPath
        {
            get
            {
                return sessionFactoryConfigPath;
            }
            set
            {
                sessionFactoryConfigPath = value;
            }
        }

        public void CreateNewSchema()
        {
            NHibernateSessionManager.Instance.CreateNewSchemaFor(sessionFactoryConfigPath);
        }

        public void DropSchema()
        {
            NHibernateSessionManager.Instance.DropSchema(sessionFactoryConfigPath);
        }                  

        public TRepository GetRepository<TRepository>(params object[] args) where TRepository : class 
        {
            return (TRepository)Activator.CreateInstance(typeof(TRepository), sessionFactoryConfigPath);
        }

        public void OnTransaction(Action action)
        {

            try
            {
                ITransaction transaction = NHibernateSessionManager.Instance.BeginTransactionOn(SessionFactoryConfigPath);
                action();
                NHibernateSessionManager.Instance.CommitTransactionOn(SessionFactoryConfigPath);
            }
            catch (HibernateException ex)
            {

                NHibernateSessionManager.Instance.CloseSessionOn(SessionFactoryConfigPath);
                throw ex;
            }

        }

        public void ClearTransaction()
        {

            NHibernateSessionManager.Instance.ClearTransactionOn(sessionFactoryConfigPath);

        }

        public void UpdateSchema()
        {
            NHibernateSessionManager.Instance.UpdateSchema(sessionFactoryConfigPath);
        }

        #region IRepositoryFactory Members


        public void Flush()
        {
            NHibernateSessionManager.Instance.Flush(sessionFactoryConfigPath);
        }

        #endregion

         

        public ISession OpenSession()
        {
            return NHibernateSessionManager.Instance.GetSessionFrom(sessionFactoryConfigPath);
        }

        public void CloseSession()
        {
            NHibernateSessionManager.Instance.CloseSessionOn(sessionFactoryConfigPath);
        }


        public Configuration GetConfiguration()
        {
            return NHibernateSessionManager.Instance.GetConfiguration(sessionFactoryConfigPath);
        }









    }
}
