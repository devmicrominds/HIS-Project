using HIS.Web.UI;
using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Json;
using System.Text;
 

namespace HIS.Web.UI.Site
{
    public partial class Base :   MasterPage    
    {
        protected void Page_Load(object sender, EventArgs e)  {

             
        
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var page = this.Page;    
        }
    }
}