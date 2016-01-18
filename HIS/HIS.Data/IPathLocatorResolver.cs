using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data
{
    public interface IPathLocatorResolver
    {
        string GetFromPath(string pathToPhysicalDocument);
    }
}
