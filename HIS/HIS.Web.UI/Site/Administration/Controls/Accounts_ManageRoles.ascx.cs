using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Administration.Controls
{


    public partial class Accounts_ManageRoles : BaseUserControl<AccountsManageRolesPresenter, IAccountsManageRolesView>, IAccountsManageRolesView
    {
        public dynamic Model { get; set; }
    }
}