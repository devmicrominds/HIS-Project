using HIS.DataAccess;
using HIS.Web.UI.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI 
{
    public class Accounts : BasePage<AccountsPresenter, IAccountsView>, IAccountsView
    {
        public CallbackPanel ControlPanel { get; set; }
        public PlaceHolder ControlPlaceHolder { get; set; }
    }
}