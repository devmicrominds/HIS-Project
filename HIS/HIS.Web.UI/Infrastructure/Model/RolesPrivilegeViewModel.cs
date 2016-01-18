using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class RolesPrivilegeViewModel   {

        public Guid RoleId { get; set; }
        public Guid PrivilegeId { get; set; } 
        public string PrivilegeDesc { get; set; }
        public bool Accessibility { get; set; }

    }
}