using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Web.UI 
{
    public interface IWebControl   : IView
    {
        bool IsCreateHierarchy { get; set; }
        /// <summary>
        /// Hierarchizes the current instance into the hierarchy of the specified presenter.
        /// </summary>
        /// <param name="shouldBeParent">The logical parent presenter.</param>
        void Hierarchize(Presenter shouldBeParent);
    }
}
