using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScreenResolutionMap : ClassMapping<ScreenResolution>
    {
        public ScreenResolutionMap() {
        
            Table("ScreenResolution");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<int>(x => x.Width);

            Property<int>(x => x.Height);

            ManyToOne<ScreenOrientation>(x => x.Orientation, m =>
            {
                m.Column("ScreenOrientationId");
                m.Cascade(Cascade.Persist);
                m.Lazy(LazyRelation.Proxy);

            });

            Set<ScreenType>(x => x.ScreenTypes, m =>
            {    
                m.Key(km => km.Column(col => col.Name("ScreenResolutionId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());

        }
    }
}
