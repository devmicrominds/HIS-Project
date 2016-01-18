using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ScreenOrientation  : BaseDomain
    {
        
        public virtual Orientation Orientation { get; set; }

        public virtual ICollection<ScreenResolution> ScreenResolutions { get; set; }
    }
}
