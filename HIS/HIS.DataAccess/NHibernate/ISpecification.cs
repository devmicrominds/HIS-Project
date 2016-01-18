using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Criterion;

namespace HIS.DataAccess
{
    public interface ISpecification
    {
        DetachedCriteria GetCriteria();
        ISpecification OrderBy(string propertyName, bool ascending);
    }
}
