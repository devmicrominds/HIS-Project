using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ResourceStream : Resource
    {
        public virtual string Location { get; set; }

        public virtual string Login { get; set; }

        public virtual string Password { get;set;}

        public override string ResourcePath
        {
            get
            {
                return base.ResourcePath;
            }
            set
            {
                base.ResourcePath = value;
            }
        }
    }
}
