using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ScreenType   : BaseDomain    {

        public ScreenType() {

            this.ScreenDivisions = new HashSet<ScreenDivision>();
        }

        public virtual string Name { get; set; }

        public virtual ICollection<ScreenDivision> ScreenDivisions { get; set; }

        public virtual IEnumerable<Channel> CreateChannels() {

            return this.ScreenDivisions.Select(o => new Channel() { 
                ScreenDivision = o 
            });
        }

        public virtual ScreenResolution ScreenResolution { get; set; }
    }
}
