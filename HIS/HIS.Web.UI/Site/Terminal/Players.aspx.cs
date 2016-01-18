using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Terminal
{
    public partial class Players : BasePage<PlayersPresenter, IPlayersView>, IPlayersView
    {
        public CallbackPanel ControlPanel { get; set; }
        public PlaceHolder ControlPlaceHolder { get; set; }
    }
}