using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data 
{
    public class RolesPrivileges : BaseDomain
    {
        public virtual Roles Role { get; set; }

        public virtual Privileges Privileges { get; set; }
    }
}
