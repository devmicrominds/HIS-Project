using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Web.UI 
{
    public interface IResourcesMenuSearchView : IView
    {
        dynamic Model { get; set; }
    }
}
