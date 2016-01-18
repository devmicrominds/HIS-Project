using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace HIS.Data.Mapping
{
    public class FontsMap : ClassMapping<Fonts>
    {
        public FontsMap()
        {
            Table("Fonts");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.FontName);

        }
    }
}
