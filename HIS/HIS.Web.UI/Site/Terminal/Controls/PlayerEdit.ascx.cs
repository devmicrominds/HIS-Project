using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Terminal.Controls
{

    public partial class PlayerEdit : BaseUserControl<PlayerEditPresenter, IPlayerEditView>, IPlayerEditView
    {
        public dynamic Model { get; set; }

    }
}