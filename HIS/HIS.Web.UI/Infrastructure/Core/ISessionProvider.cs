using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Web.UI 
{
    public interface ISessionProvider
    {
        object this[string name] { get; set; }
        object this[int index] { get; set; }

        bool Contains(string name);
        void Remove(string name);
    }
}
