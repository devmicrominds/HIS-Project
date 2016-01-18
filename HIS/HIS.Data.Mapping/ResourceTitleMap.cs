using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace HIS.Data.Mapping
{
    public class ResourceTitleMap : ClassMapping<ResourceTitle>
    {
        public ResourceTitleMap()
        {
            Table("ResourceTitle");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            Property<bool>(x => x.Display);

            Property<string>(x => x.Title);

            Property<DateTime>(x => x.CreateDate);

            Property<Guid>(x => x.CreatedBy);

            Property<DateTime?>(x => x.UpdateDate);

            Property<Guid>(x => x.UpdatedBy);

            Property<int>(x => x.ResourceType);
            
        }
    }
}
