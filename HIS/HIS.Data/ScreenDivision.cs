using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ScreenDivision  : BaseDomain
    {
      

        public virtual string Name { get; set; }

        public virtual int X { get; set; }

        public virtual int Y { get; set; }

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public virtual ScreenType ScreenType { get; set; }
    }
}
