using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping.Utils
{
    public class PathLocatorResolver: IPathLocatorResolver
    {
        private readonly ISession session;

        public PathLocatorResolver(ISession session) {

            if (null == session)
                throw new ArgumentNullException("session is null!");
            this.session = session;
        }

        public string GetFromPath(string pathToPhysicalDocument)
        {
            using (session) {

                return this.session
                       .CreateSQLQuery(String.Format("Select GetPathLocator('{0}').ToString()", pathToPhysicalDocument))
                       .UniqueResult<string>();
            }
        }
    }
}
