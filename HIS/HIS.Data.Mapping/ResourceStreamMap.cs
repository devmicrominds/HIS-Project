using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ResourceStreamMap : JoinedSubclassMapping<ResourceStream>
    {
        public ResourceStreamMap() {

            Table("ResourcesStream");

            Key(m => m.Column("Id"));

            Property<string>(x => x.Location);

            Property<string>(x => x.Login);

            Property<string>(x => x.Password);
        }
    }
}
