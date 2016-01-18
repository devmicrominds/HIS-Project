using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.ZScript
{
    public interface ITestjs01View : IBasePage
    {
        CallbackPanel ControlPanel { get; set; }
        PlaceHolder ControlPlaceHolder { get; set; }
    }
}
