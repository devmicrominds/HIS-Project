using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScreenOrientationMap : ClassMapping<ScreenOrientation>
    {
        public ScreenOrientationMap() {

            Table("ScreenOrientation");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<Orientation>(x => x.Orientation);

            Set<ScreenResolution>(x => x.ScreenResolutions, m =>
            {

                m.Key(km => km.Column(col => col.Name("ScreenOrientationId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());
        }
    }
}
