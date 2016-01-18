using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data 
{
    public class Privileges : BaseDomain
    {
        

        public virtual string Code { get; set; }

        public virtual string PrivilegeDesc { get; set; }
    }
}
