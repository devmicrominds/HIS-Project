using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Terminal.Controls
{
    public partial class Group_Manage : BaseUserControl<GroupManagePresenter, IGroupManageView>, IGroupManageView
    {
        public dynamic Model { get; set; }
    }

}