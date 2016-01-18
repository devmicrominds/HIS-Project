using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Web.UI.Site.Terminal.Controls
{
    public interface IGroupManageView : IView
    {
        dynamic Model { get; set; }
    }

}
