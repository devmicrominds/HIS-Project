using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Administration.controls
{
    public partial class Accounts_Users : BaseUserControl<AccountsUsersPresenter, IAccountsUsersView>,
        IAccountsUsersView
    {
        public GridView MainGrid { 
            get  { 
                return __grid; 
            }
        }

        public dynamic Model { get; set; }

        public override PageSettings LocalPageSettings
        {
            get
            {

                return new PageSettings(MainGrid.PageSize, MainGrid.PageIndex);
            }
            set
            {

                MainGrid.PageIndex = value.PageIndex;
                MainGrid.PageSize = value.PageSize;

            }
        }

       


    }


}