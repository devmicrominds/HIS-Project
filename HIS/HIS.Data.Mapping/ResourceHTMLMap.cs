using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ResourceHTMLMap : JoinedSubclassMapping<ResourceHTML>
    {
        public ResourceHTMLMap() {

            Table("ResourcesHTML");

            Key(m => m.Column("Id"));

            Property<string>(x => x.Location);
        }
    }
}
