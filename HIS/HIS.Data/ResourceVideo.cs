using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public class ResourceVideo : Resource
    {        
        public virtual string Title { get; set; }

        public virtual string Filename { get; set; }

        public virtual string Extension { get; set; }

        public virtual string Location { get; set; }

        public virtual string Size { get; set; }

        public virtual string Width { get; set; }

        public virtual string Height { get; set; }

        public virtual string Length { get; set; }

        public virtual string Dimension { get; set; }

        public virtual byte[] Thumbnail { get; set; }

        public override string ResourcePath
        {
            get
            {
                return this.Filename;
            }
            
        }
         
    }
}
