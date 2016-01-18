using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace HIS.Data.Mapping
{    
    public class FontSizeMap : ClassMapping<FontSize>
    {
        public FontSizeMap()
        {
            Table("FontSize");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<decimal>(x => x.Size);
            
        }
    }
}
