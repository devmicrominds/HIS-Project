using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI 
{
    public partial class EditableText : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
       
        public string DataID { get; set; }

        public string DataValue { get; set; }

        public bool Disabled { get; set; }

        public int MaxLength { get; set; }

        public string InputClass { get; set; }

        public string Validate { get; set; }
    }
}