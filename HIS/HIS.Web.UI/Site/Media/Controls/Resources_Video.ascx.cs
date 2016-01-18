using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Media.Controls
{
    public partial class Resources_Video : BaseUserControl<ResourcesVideoPresenter,IResourcesVideoView>,IResourcesVideoView
    {
        public GridView Grid
        {
            get { return __grid; }
        }

        public dynamic Model { get; set; }
       
    }
}