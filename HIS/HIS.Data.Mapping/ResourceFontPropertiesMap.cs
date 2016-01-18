using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace HIS.Data.Mapping
{
    public class ResourceFontPropertiesMap : ClassMapping<ResourceFontProperties>
    {
        public ResourceFontPropertiesMap()
        {
            Table("ResourceFontProperties");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<Guid>(x => x.MId);

            Property<float>(x => x.FontSize);

            Property<string>(x => x.ForeColor);

            Property<string>(x => x.BackgroundColor);

            Property<string>(x => x.Font);
            
        }
    }
}
