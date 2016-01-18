using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HIS.Data
{
    [Serializable]
    public class Users : BaseDomain
    {    
        public Users() { }

       
        public virtual string UserName { get; set; }
        public virtual string UserPassword { get; set; }
        public virtual string Email { get; set; }

        public virtual Roles UserRoles { get; set; }


        
    }



}
