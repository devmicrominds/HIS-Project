using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.DataAccess
{
    public interface IRepositoryFactory  
    {
        
        TRepository GetRepository<TRepository>(params object[] args)where TRepository : class;

        void OnTransaction(Action action);

        void ClearTransaction();
        void CreateNewSchema();
        void DropSchema();
        void UpdateSchema();
        void CloseSession();

        
        NHibernate.ISession OpenSession();

        NHibernate.Cfg.Configuration GetConfiguration();

        void Flush();
    }
}
