using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Test
{
    public partial class jsonTest : BasePage<jsonTestPresenter, IjsonTestView>, IjsonTestView
    {
        public CallbackPanel ControlPanel { get; set; }
        public PlaceHolder ControlPlaceHolder { get; set; }
    }

}