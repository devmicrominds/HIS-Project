using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Web.UI.Site.Administration.Controls
{
    public interface IAccountsAddEditView : IView
    {
        dynamic Model { get; set; }
    }
}
