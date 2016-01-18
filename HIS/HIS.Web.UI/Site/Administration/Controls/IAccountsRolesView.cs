using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace HIS.Web.UI
{
    public interface IAccountsRolesView : IView
    {    
        GridView MainGrid { get; set; }
        dynamic Model { get; set; }
    }
}
