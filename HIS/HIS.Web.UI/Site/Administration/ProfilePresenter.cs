using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Administration
{
    public class ProfilePresenter : GenericPresenter<IProfileView>
    {
        public override void InitialRun()
        {


        }

        public override void PartialRender()
        {


        }
    }

    public interface IProfileView : IBasePage, IView
    {
        CallbackPanel ControlPanel { get; set; }
        PlaceHolder ControlPlaceHolder { get; set; }
    }

}