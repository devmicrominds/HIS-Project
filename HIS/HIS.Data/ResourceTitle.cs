using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ResourceTitle : BaseDomain
    {        
        public virtual string Name { get; set; }

        public virtual bool Display { get; set; }

        public virtual string Title { get; set; }

        public virtual DateTime CreateDate { get; set; }

        public virtual Guid CreatedBy { get; set; }

        public virtual DateTime? UpdateDate { get; set; }

        public virtual Guid UpdatedBy { get; set; }

        public virtual int ResourceType { get; set; }

    }
}
