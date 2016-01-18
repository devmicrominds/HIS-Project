using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScreenDivisionMap : ClassMapping<ScreenDivision>
    {
        public ScreenDivisionMap () {

            Table("ScreenDivision");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            Property<int>(x => x.X);

            Property<int>(x => x.Y);

            Property<int>(x => x.Width);

            Property<int>(x => x.Height);

            ManyToOne<ScreenType>(x => x.ScreenType, m => {

                m.Column("ScreenTypeId");

                m.Cascade(Cascade.Persist);
            });
             
        }
    }
}
