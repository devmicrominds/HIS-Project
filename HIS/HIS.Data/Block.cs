using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Block : BaseDomain     {   

        public virtual int Length { get; set; }

        public virtual Resource Resource { get; set; }

        public virtual Channel Channel { get; set; }

    }
}
