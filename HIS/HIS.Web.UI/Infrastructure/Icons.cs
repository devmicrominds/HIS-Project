using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI 
{
    public class Icons
    {
        HTML5Icon html5icon;
        
        public Icons(HTML5Icon html5icon)
        {
            this.html5icon = html5icon;
        }

        public HTML5Icon HTML5
        {
            get { return html5icon; }
        }
    }
}