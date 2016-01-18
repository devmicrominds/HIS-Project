using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class Ticker : BaseDomain
    {
        public virtual Guid MId { get; set; }

        public virtual string Title { get; set; }

        public virtual bool Display { get; set; }

        public virtual int OrderBy { get; set; }        
    }
}
