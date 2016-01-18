using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ResourceMusic: Resource  {

        public virtual string Artist { get; set; }

        public virtual string Album { get; set; }

        public virtual string Genre { get; set; }

        public virtual string Filename { get; set; }

        public virtual string Extension { get; set; }    

        public virtual string Location { get; set; }

        public virtual string Size { get; set; }

        public virtual string Length { get; set; }

        public override string ResourcePath
        {
            get
            {
                return this.Filename;
            }
             
        }

    }
}
