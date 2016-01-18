using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ScreenResolution : BaseDomain {

       

        public virtual int Width { get; set; }

        public virtual int Height { get; set; }

        public virtual string Resolution
        {
            get { return String.Format("{0}x{1}", this.Width, this.Height); }
        }

        public virtual ScreenOrientation Orientation { get; set; }

        public virtual ICollection<ScreenType> ScreenTypes { get; set; }



        public virtual void AddScreenType(ScreenType screentype)
        {
            this.ScreenTypes.Add(screentype);
            screentype.ScreenResolution = this;
             
        }
    }
}
