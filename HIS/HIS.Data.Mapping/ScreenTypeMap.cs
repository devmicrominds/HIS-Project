using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.Data.Mapping
{
    public class ScreenTypeMap : ClassMapping<ScreenType>
    {
        public ScreenTypeMap () {
            
            Table("ScreenType");

            Id(x => x.Id, map => map.Generator(Generators.GuidComb));

            Property<string>(x => x.Name);

            ManyToOne<ScreenResolution>(x => x.ScreenResolution, m => {

                m.Column("ScreenResolutionId");
                m.Cascade(Cascade.Persist);
               
            });

            Set<ScreenDivision>(x => x.ScreenDivisions, m =>
            {
                m.Key(km => km.Column(col => col.Name("ScreenTypeId")));

                m.Cascade(Cascade.All | Cascade.DeleteOrphans);

                m.Inverse(true);

                m.Lazy(CollectionLazy.Lazy);

            }, a => a.OneToMany());

        }
    }
}
