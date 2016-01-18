using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Roles : BaseDomain
    {
        public Roles()
        {
            this.RolePrivileges = new HashSet<Privileges>();
            this.UserRoles = new HashSet<Users>();
        }


        public virtual string Name { get; set; }
        public virtual string Description { get; set; }

        public virtual ICollection<Privileges> RolePrivileges { get; set; }
        public virtual ICollection<Users> UserRoles { get; set; }

        public virtual void AddPrivilege(Privileges priv)
        {
            this.RolePrivileges.Add(priv);

        }

        public virtual void RemovePrivilege(Privileges priv)
        {
            this.RolePrivileges.Remove(priv);

        }
    }
}
