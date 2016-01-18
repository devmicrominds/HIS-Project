using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Administration.Controls
{
    public partial class Accounts_AddEdit : BaseUserControl<AccountsAddEditPresenter, IAccountsAddEditView>, IAccountsAddEditView
    {
        public dynamic Model { get; set; }
    }
}