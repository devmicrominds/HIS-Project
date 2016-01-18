using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Player : BaseDomain   {

        public virtual string Name { get; set; }

        public virtual string Location { get; set; }

        public virtual string IPAddress { get; set; }

        public virtual PlayerSettings PlayerSettings { get; set; }

        public virtual Groups Groups { get; set; }

    }
}
