using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class MediaCategory : BaseDomain  {

        public MediaCategory() {

            this.Resources = new HashSet<Resource>();
        }

        public virtual ResourceType MediaResourceType { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ColorCode { get; set; }

        public virtual ICollection<Resource> Resources { get; set; }

        public virtual Resource AddResources(Resource resource) {

            if (null == this.Resources)
                this.Resources = new HashSet<Resource>();

            this.Resources.Add(resource);
            
            resource.MediaCategory = this;

            return resource;
        }
    }
}
