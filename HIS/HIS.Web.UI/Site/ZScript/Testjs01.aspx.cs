using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.ZScript
{
    public partial class Testjs01 : BasePage<Testjs01Presenter, ITestjs01View>, ITestjs01View
    {
        public CallbackPanel ControlPanel { get; set; }
        public PlaceHolder ControlPlaceHolder { get; set; }
    }
}