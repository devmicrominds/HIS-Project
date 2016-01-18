using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class UsersRoles
    {
        public virtual Users User { get; set; }

        public virtual Roles Role { get; set; }
        
    }
}
