using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HIS.Web.UI.Site.Administration.controls
{
    public partial class Accounts_Roles : BaseUserControl<AccountsRolesPresenter,IAccountsRolesView>,IAccountsRolesView
    {
        public GridView MainGrid
        {
            get
            {
                return uGrid;
            }
            set
            {
                uGrid = value;
            }
        }

        public dynamic Model { get; set; }

        public override PageSettings LocalPageSettings
        {
            get
            {
                return new PageSettings(uGrid.PageSize, uGrid.PageIndex);
            }
            set
            {

                uGrid.PageIndex = value.PageIndex;
                uGrid.PageSize = value.PageSize;

            }
        }

        
    }
}