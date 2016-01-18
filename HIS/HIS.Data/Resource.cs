using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Resource : BaseDomain {

        public virtual ResourceType ResourceType { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual MediaCategory MediaCategory { get; set; }

        public virtual string ResourcePath { get; set; }
    }
}
