using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.DataAccess
{
    public class Repository<T> : AbstractNHibernateDao<T> where T : class
    {

        public Repository(string sessionFactoryConfigPath) :
            base(sessionFactoryConfigPath)
        {

        }

       
    }

    public class Repository : AbstractNHibernateDao<dynamic>
    {

        public Repository(string sessionFactoryConfigPath) :
            base(sessionFactoryConfigPath)
        {

        }
    }
}
