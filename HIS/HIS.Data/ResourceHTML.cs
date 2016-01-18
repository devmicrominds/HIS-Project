using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ResourceHTML : Resource
    {
        public virtual string Location { get; set; }

        public override string ResourcePath
        {
            get
            {
                return this.Location;
            }
            set
            {
                this.Location = value;
            }
        }
    }
}
