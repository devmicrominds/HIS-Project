using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Media.Controls
{
    public partial class Resources_Stream : BaseUserControl<ResourcesStreamPresenter,IResourcesStreamView>,IResourcesStreamView
    {

        public dynamic Model
        {
            get;
            set;
        }

        public GridView Grid
        {
            get { return __grid; }
        }
    }
}