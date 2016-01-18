using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Media.Controls
{
    public partial class Resources_Menu : BaseUserControl<ResourcesMenuPresenter,IResourcesMenuView>,IResourcesMenuView {
          

        public PlaceHolder MediaPlaceHolder { get; set; }

        public bool IsInitialRun  { get; set; }
       
    }
}