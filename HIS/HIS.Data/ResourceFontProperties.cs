using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ResourceFontProperties : BaseDomain 
    {
        public virtual Guid MId { get; set; }

        public virtual float FontSize { get; set; }

        public virtual string ForeColor { get; set; }

        public virtual string BackgroundColor { get; set; }

        public virtual string Font { get; set; }        
    }
}
